using eVet.Model;
using eVet.Model.Enums;
using eVet.Model.Exceptions;
using eVet.Services.Database;
using eVet.Services.Termini;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Konstante = eVet.Model.Constants;

namespace eVet.Services.Placanja
{
    public class PlacanjaService : IPlacanjaService
    {
        private const string PayPalCompleted = "COMPLETED";

        private readonly EVetDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPayPalClient _payPal;
        private readonly ICurrentUserService _currentUser;
        private readonly INotifikacijeService _notifikacije;
        private readonly PayPalSettings _settings;

        public PlacanjaService(EVetDbContext context, IMapper mapper, IPayPalClient payPal, ICurrentUserService currentUser, INotifikacijeService notifikacije, IOptions<PayPalSettings> settings)
        {
            _context = context;
            _mapper = mapper;
            _payPal = payPal;
            _currentUser = currentUser;
            _notifikacije = notifikacije;
            _settings = settings.Value;
        }

        public async Task<PayPalNarudzba> ZapocniPayPalAsync(int terminId, CancellationToken cancellationToken = default)
        {
            int korisnikId = _currentUser.KorisnikId;
            var termin = await _context.Termini
                .Include(x => x.Uplate)
                .FirstOrDefaultAsync(x => x.TerminId == terminId && x.KorisnikId == korisnikId, cancellationToken)
                ?? throw new NotFoundException("Termin ne postoji.");

            if (termin.NacinPlacanjaId != Konstante.NaciniPlacanja.PayPal)
            {
                throw new BusinessException("Za ovaj termin je odabrano plaćanje gotovinom u stanici.");
            }

            if (!TerminStateMachine.AktivniStatusi.Contains(termin.Status))
            {
                throw new BusinessException("Moguće je platiti samo termin koji čeka potvrdu ili je potvrđen.");
            }

            if (termin.Uplate.Any(x => x.Status == StatusUplate.Placeno))
            {
                throw new BusinessException("Termin je već plaćen.");
            }

            // Sprječavanje višestrukog plaćanja: otvorena uplata se nastavlja umjesto da se kreira nova.
            var otvorena = termin.Uplate.FirstOrDefault(x => x.Status == StatusUplate.NaCekanju);
            if (otvorena != null)
            {
                var postojeci = await _payPal.GetOrderAsync(otvorena.PayPalOrderId, cancellationToken);
                if (postojeci.Status is "CREATED" or "PAYER_ACTION_REQUIRED" && postojeci.ApproveUrl != null)
                {
                    return Narudzba(otvorena, postojeci.ApproveUrl);
                }

                if (postojeci.Status is "APPROVED" or PayPalCompleted)
                {
                    throw new BusinessException("Plaćanje je već odobreno u PayPalu i čeka potvrdu. Osvježite termin.");
                }

                // Order je istekao ili poništen u PayPalu; zatvara se prije otvaranja novog.
                otvorena.Status = StatusUplate.Neuspjesno;
                await _context.SaveChangesAsync(cancellationToken);
            }

            var iznos = Math.Round(termin.UkupnaCijena * _settings.KursKonverzije, 2, MidpointRounding.AwayFromZero);
            if (iznos <= 0)
            {
                throw new BusinessException("Termin nema iznos za plaćanje.");
            }

            var order = await _payPal.KreirajOrderAsync($"termin-{termin.TerminId}", $"PetCity termin {termin.Sifra}", iznos, _settings.Valuta, cancellationToken);
            if (order.ApproveUrl == null)
            {
                throw new BusinessException("PayPal nije vratio adresu za plaćanje. Pokušajte ponovo.");
            }

            var uplata = new Uplata
            {
                Termin = termin,
                Iznos = iznos,
                Valuta = _settings.Valuta,
                Status = StatusUplate.NaCekanju,
                PayPalOrderId = order.Id,
                DatumKreiranja = DateTime.UtcNow
            };
            _context.Uplate.Add(uplata);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                // Jedinstveni indeks dozvoljava samo jednu otvorenu ili plaćenu uplatu po terminu (paralelni zahtjevi).
                throw new BusinessException("Plaćanje za ovaj termin je već u toku. Osvježite termin i pokušajte ponovo.");
            }

            return Narudzba(uplata, order.ApproveUrl);
        }

        public async Task<Model.Termini> PotvrdiPayPalAsync(int uplataId, CancellationToken cancellationToken = default)
        {
            int korisnikId = _currentUser.KorisnikId;
            var uplata = await _context.Uplate
                .Include(x => x.Termin).ThenInclude(x => x.Korisnik)
                .FirstOrDefaultAsync(x => x.UplataId == uplataId && x.Termin.KorisnikId == korisnikId, cancellationToken)
                ?? throw new NotFoundException("Uplata ne postoji.");

            // Idempotentnost: već potvrđena uplata vraća trenutno stanje bez ponovnih efekata.
            if (uplata.Status == StatusUplate.Placeno)
            {
                return await UcitajTerminAsync(uplata.TerminId, cancellationToken);
            }

            if (uplata.Status != StatusUplate.NaCekanju)
            {
                throw new BusinessException("Ova uplata više nije aktivna. Pokrenite plaćanje ponovo.");
            }

            // Naplatu izvršava i provjerava server; klijent samo javlja da se vratio iz PayPala.
            var order = await _payPal.CaptureAsync(uplata.PayPalOrderId, $"capture-{uplata.UplataId}", cancellationToken);
            var capture = order.Capture;
            if (order.Status != PayPalCompleted || capture == null || capture.Status != PayPalCompleted)
            {
                throw new BusinessException("PayPal nije potvrdio naplatu. Pokušajte ponovo.");
            }

            // Uslovni update: ako je paralelni zahtjev već potvrdio uplatu, efekti se ne ponavljaju.
            var sada = DateTime.UtcNow;
            int azurirano = await _context.Uplate
                .Where(x => x.UplataId == uplata.UplataId && x.Status == StatusUplate.NaCekanju)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.Status, StatusUplate.Placeno)
                    .SetProperty(x => x.PayPalCaptureId, capture.Id)
                    .SetProperty(x => x.Iznos, capture.Iznos)
                    .SetProperty(x => x.Valuta, capture.Valuta)
                    .SetProperty(x => x.DatumPlacanja, sada), cancellationToken);

            if (azurirano == 1)
            {
                _notifikacije.Dodaj(uplata.Termin.Korisnik, "Termin je plaćen",
                    $"Uplata za termin {uplata.Termin.Sifra} u iznosu od {capture.Iznos:0.00} {capture.Valuta} je uspješno izvršena putem PayPala.");
                await _context.SaveChangesAsync(cancellationToken);
                await _notifikacije.PosaljiEmailoveAsync(cancellationToken);
            }

            return await UcitajTerminAsync(uplata.TerminId, cancellationToken);
        }

        public async Task ObradiOtkazivanjeAsync(Termin termin, CancellationToken cancellationToken = default)
        {
            foreach (var uplata in termin.Uplate)
            {
                if (uplata.Status == StatusUplate.NaCekanju)
                {
                    uplata.Status = StatusUplate.Neuspjesno;
                }
                else if (uplata.Status == StatusUplate.Placeno && uplata.PayPalCaptureId != null)
                {
                    // Vraća se stvarno naplaćeni iznos iz PayPala, a ne ponovo izračunata cijena termina.
                    var refund = await _payPal.RefundAsync(uplata.PayPalCaptureId, uplata.Iznos, uplata.Valuta, $"refund-{uplata.UplataId}", cancellationToken);

                    uplata.Status = StatusUplate.Refundirano;
                    uplata.IznosRefunda = refund.Iznos;
                    uplata.PayPalRefundId = refund.Id;
                    uplata.DatumRefunda = DateTime.UtcNow;

                    _notifikacije.Dodaj(termin.Korisnik, "Novac je vraćen",
                        $"Za otkazani termin {termin.Sifra} vraćeno vam je {refund.Iznos:0.00} {refund.Valuta} na PayPal račun.");
                }
            }
        }

        private PayPalNarudzba Narudzba(Uplata uplata, string approveUrl) => new()
        {
            UplataId = uplata.UplataId,
            OrderId = uplata.PayPalOrderId,
            ApproveUrl = approveUrl,
            ReturnUrl = _settings.ReturnUrl,
            CancelUrl = _settings.CancelUrl,
            Iznos = uplata.Iznos,
            Valuta = uplata.Valuta
        };

        private Task<Model.Termini> UcitajTerminAsync(int terminId, CancellationToken cancellationToken)
        {
            return _context.Termini
                .Where(x => x.TerminId == terminId)
                .ProjectToType<Model.Termini>(_mapper.Config)
                .FirstAsync(cancellationToken);
        }
    }
}
