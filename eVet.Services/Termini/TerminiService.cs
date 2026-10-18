using eVet.Model.Constants;
using eVet.Model.Enums;
using eVet.Model.Exceptions;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Security.Cryptography;

namespace eVet.Services.Termini
{
    public class TerminiService : BaseService<Model.Termini, TerminiSearchObject, Termin>, ITerminiService
    {
        private const string SifraZnakovi = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        private const int SifraDuzina = 8;

        private readonly ICurrentUserService _currentUser;
        private readonly TerminiSettings _settings;
        private readonly TimeZoneInfo _vremenskaZona;

        public TerminiService(EVetDbContext context, IMapper mapper, ICurrentUserService currentUser, IOptions<TerminiSettings> settings) : base(context, mapper)
        {
            _currentUser = currentUser;
            _settings = settings.Value;
            _vremenskaZona = TimeZoneInfo.FindSystemTimeZoneById(_settings.VremenskaZona);
        }

        protected override string NazivEntiteta => "Termin";

        private TimeOnly RadnoVrijemeOd => new(_settings.RadnoVrijemeOdSat, 0);

        private TimeOnly RadnoVrijemeDo => _settings.RadnoVrijemeDoSat == 24 ? TimeOnly.MaxValue : new(_settings.RadnoVrijemeDoSat, 0);

        private DateTime LokalnoSada => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _vremenskaZona);

        // Vlasnik vidi sve termine, veterinar svoje zakazane termine, klijent svoje rezervacije.
        protected override IQueryable<Termin> ApplyPristup(IQueryable<Termin> query)
        {
            if (_currentUser.IsInRole(Uloge.Vlasnik))
            {
                return query;
            }

            int korisnikId = _currentUser.KorisnikId;

            if (_currentUser.IsInRole(Uloge.Veterinar))
            {
                return query.Where(x => x.VeterinarId == korisnikId);
            }

            return query.Where(x => x.KorisnikId == korisnikId);
        }

        protected override IQueryable<Termin> AddFilter(TerminiSearchObject search, IQueryable<Termin> query)
        {
            if (!string.IsNullOrWhiteSpace(search.FTS))
            {
                query = query.Where(x => x.Sifra.Contains(search.FTS)
                    || x.Ljubimac.Ime.Contains(search.FTS)
                    || x.Korisnik.Ime.Contains(search.FTS)
                    || x.Korisnik.Prezime.Contains(search.FTS));
            }

            if (search.Status.HasValue)
            {
                query = query.Where(x => x.Status == search.Status);
            }

            if (search.DatumOd.HasValue)
            {
                query = query.Where(x => x.DatumTermina >= search.DatumOd);
            }

            if (search.DatumDo.HasValue)
            {
                query = query.Where(x => x.DatumTermina <= search.DatumDo);
            }

            if (search.VeterinarId.HasValue)
            {
                query = query.Where(x => x.VeterinarId == search.VeterinarId);
            }

            if (search.LjubimacId.HasValue)
            {
                query = query.Where(x => x.LjubimacId == search.LjubimacId);
            }

            return query;
        }

        public async Task<Model.Termini> KreirajAsync(TerminInsertRequest request, CancellationToken cancellationToken = default)
        {
            int klijentId = _currentUser.KorisnikId;
            var datum = request.DatumTermina!.Value;
            var pocetak = request.VrijemePocetka!.Value;

            ProvjeriDatumIVrijeme(datum, pocetak);

            var klijent = await Context.Korisnici.FirstAsync(x => x.KorisnikId == klijentId, cancellationToken);

            var ljubimac = await Context.Ljubimci.FirstOrDefaultAsync(x => x.LjubimacId == request.LjubimacId && x.KorisnikId == klijentId, cancellationToken)
                ?? throw new BusinessException("Odabrani ljubimac ne postoji ili nije registrovan na vaše ime.");

            var veterinar = await AktivniVeterinari().FirstOrDefaultAsync(x => x.KorisnikId == request.VeterinarId, cancellationToken)
                ?? throw new BusinessException("Odabrani veterinar ne postoji ili trenutno ne prima termine.");

            var uslugeIds = request.UslugeIds.Distinct().ToList();
            var usluge = await Context.Usluge.Where(x => uslugeIds.Contains(x.UslugaId)).ToListAsync(cancellationToken);
            if (usluge.Count != uslugeIds.Count)
            {
                throw new BusinessException("Neka od usluga u korpi više nije dostupna. Osvježite korpu i pokušajte ponovo.");
            }

            var nacinPlacanja = await Context.NaciniPlacanja.FirstOrDefaultAsync(x => x.NacinPlacanjaId == request.NacinPlacanjaId, cancellationToken)
                ?? throw new BusinessException("Odabrani način plaćanja ne postoji.");

            Grad? gradPrevoza = null;
            if (request.JePrevoz)
            {
                gradPrevoza = await Context.Gradovi.FirstOrDefaultAsync(x => x.GradId == request.GradPrevozaId, cancellationToken)
                    ?? throw new BusinessException("Odabrani grad za prevoz ne postoji.");
            }

            int ukupnoTrajanje = usluge.Sum(x => x.Trajanje);
            var kraj = IzracunajKraj(datum, pocetak, ukupnoTrajanje);

            var aktiviranaPromocija = await UcitajPromocijuAsync(request.KodPromocije, klijentId, usluge, cancellationToken);

            var termin = new Termin
            {
                Korisnik = klijent,
                Veterinar = veterinar,
                Ljubimac = ljubimac,
                NacinPlacanja = nacinPlacanja,
                Sifra = GenerisiSifru(),
                DatumTermina = datum,
                VrijemePocetka = pocetak,
                VrijemeKraja = kraj,
                UkupnoTrajanje = ukupnoTrajanje,
                Status = StatusTermina.NaCekanju,
                JePrevoz = request.JePrevoz,
                AdresaPrevoza = request.JePrevoz ? request.AdresaPrevoza : null,
                GradPrevoza = gradPrevoza,
                DatumKreiranja = DateTime.UtcNow,
                AktiviranaPromocija = aktiviranaPromocija,
                StavkeTermina = usluge.Select(x => new StavkaTermina { Usluga = x, Cijena = x.Cijena }).ToList()
            };

            IzracunajCijenu(termin, usluge, aktiviranaPromocija);

            if (aktiviranaPromocija != null)
            {
                aktiviranaPromocija.Iskoristena = true;
            }

            NotifikacijeHelper.Dodaj(Context, veterinar.KorisnikId, "Novi zahtjev za termin",
                $"{klijent.Ime} {klijent.Prezime} je zatražio/la termin {termin.Sifra} za ljubimca {ljubimac.Ime}, {Formatiraj(termin)}.");
            NotifikacijeHelper.Dodaj(Context, klijentId, "Zahtjev za termin je zaprimljen",
                $"Termin {termin.Sifra} ({Formatiraj(termin)}) čeka potvrdu veterinara.");

            // Provjera zauzetosti i upis moraju biti atomarni, inače dva klijenta mogu istovremeno rezervisati isti termin.
            var strategy = Context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await Context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

                await ProvjeriZauzetostAsync(termin, cancellationToken);

                Context.Termini.Add(termin);
                await Context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            });

            return Mapper.Map<Model.Termini>(termin);
        }

        public Task<Model.Termini> PotvrdiAsync(int id, CancellationToken cancellationToken = default)
        {
            return PromijeniStatusAsync(id, StatusTermina.Potvrdjen, null, termin =>
            {
                NotifikacijeHelper.Dodaj(Context, termin.KorisnikId, "Termin je potvrđen",
                    $"Vaš termin {termin.Sifra} ({Formatiraj(termin)}) je potvrdio/la {termin.Veterinar.Ime} {termin.Veterinar.Prezime}.");
            }, cancellationToken);
        }

        public Task<Model.Termini> OdbijAsync(int id, PromjenaStatusaRequest request, CancellationToken cancellationToken = default)
        {
            return PromijeniStatusAsync(id, StatusTermina.Odbijen, request.Razlog, termin =>
            {
                ProvjeriDaNijePlacen(termin, "odbiti");
                VratiPromociju(termin);

                NotifikacijeHelper.Dodaj(Context, termin.KorisnikId, "Termin je odbijen",
                    $"Vaš zahtjev za termin {termin.Sifra} ({Formatiraj(termin)}) je odbijen. Razlog: {request.Razlog}");
            }, cancellationToken);
        }

        public Task<Model.Termini> OtkaziAsync(int id, PromjenaStatusaRequest request, CancellationToken cancellationToken = default)
        {
            return PromijeniStatusAsync(id, StatusTermina.Otkazan, request.Razlog, termin =>
            {
                bool otkazujeKlijent = termin.KorisnikId == _currentUser.KorisnikId;

                if (otkazujeKlijent)
                {
                    var pocetak = termin.DatumTermina.ToDateTime(termin.VrijemePocetka);
                    if (pocetak - LokalnoSada < TimeSpan.FromHours(_settings.MinimalnoSatiZaOtkazivanje))
                    {
                        throw new BusinessException($"Termin je moguće otkazati najkasnije {_settings.MinimalnoSatiZaOtkazivanje} sati prije početka. Za hitna otkazivanja kontaktirajte stanicu.");
                    }
                }

                ProvjeriDaNijePlacen(termin, "otkazati");
                VratiPromociju(termin);

                if (otkazujeKlijent)
                {
                    NotifikacijeHelper.Dodaj(Context, termin.VeterinarId, "Termin je otkazan",
                        $"Klijent {termin.Korisnik.Ime} {termin.Korisnik.Prezime} je otkazao/la termin {termin.Sifra} ({Formatiraj(termin)}). Razlog: {request.Razlog}");
                }
                else
                {
                    NotifikacijeHelper.Dodaj(Context, termin.KorisnikId, "Termin je otkazan",
                        $"Vaš termin {termin.Sifra} ({Formatiraj(termin)}) je otkazan od strane stanice. Razlog: {request.Razlog}");
                }
            }, cancellationToken);
        }

        public Task<Model.Termini> ZavrsiAsync(int id, CancellationToken cancellationToken = default)
        {
            return PromijeniStatusAsync(id, StatusTermina.Zavrsen, null, termin =>
            {
                if (termin.DatumTermina.ToDateTime(termin.VrijemePocetka) > LokalnoSada)
                {
                    throw new BusinessException("Termin nije moguće označiti završenim prije njegovog početka.");
                }

                NotifikacijeHelper.Dodaj(Context, termin.KorisnikId, "Termin je završen",
                    $"Termin {termin.Sifra} za ljubimca {termin.Ljubimac.Ime} je završen. Ocijenite uslugu i veterinara u historiji termina.");
            }, cancellationToken);
        }

        public async Task<List<TimeOnly>> GetSlobodnaVremenaAsync(int veterinarId, DateOnly datum, int trajanje, CancellationToken cancellationToken = default)
        {
            if (trajanje <= 0)
            {
                throw new BusinessException("Trajanje mora biti veće od 0 minuta.");
            }

            if (!await AktivniVeterinari().AnyAsync(x => x.KorisnikId == veterinarId, cancellationToken))
            {
                throw new NotFoundException("Odabrani veterinar ne postoji.");
            }

            var zauzeti = await Context.Termini
                .Where(x => x.VeterinarId == veterinarId && x.DatumTermina == datum && TerminStateMachine.AktivniStatusi.Contains(x.Status))
                .Select(x => new { x.VrijemePocetka, x.VrijemeKraja })
                .ToListAsync(cancellationToken);

            var sada = LokalnoSada;
            var slobodna = new List<TimeOnly>();

            for (var pocetak = RadnoVrijemeOd; ; pocetak = pocetak.AddMinutes(_settings.IntervalMinuta))
            {
                var pocetakDatum = datum.ToDateTime(pocetak);
                var krajDatum = pocetakDatum.AddMinutes(trajanje);

                if (krajDatum.Date != pocetakDatum.Date || TimeOnly.FromDateTime(krajDatum) > RadnoVrijemeDo || pocetak < RadnoVrijemeOd)
                {
                    break;
                }

                var kraj = TimeOnly.FromDateTime(krajDatum);
                bool jeSlobodan = pocetakDatum > sada && !zauzeti.Any(z => z.VrijemePocetka < kraj && z.VrijemeKraja > pocetak);

                if (jeSlobodan)
                {
                    slobodna.Add(pocetak);
                }
            }

            return slobodna;
        }

        private async Task<Model.Termini> PromijeniStatusAsync(int id, StatusTermina noviStatus, string? razlog, Action<Termin> poslovnaPravila, CancellationToken cancellationToken)
        {
            var termin = await ApplyPristup(Context.Termini)
                .Include(x => x.Korisnik)
                .Include(x => x.Veterinar)
                .Include(x => x.Ljubimac)
                .Include(x => x.NacinPlacanja)
                .Include(x => x.GradPrevoza)
                .Include(x => x.AktiviranaPromocija)
                .Include(x => x.Recenzija)
                .Include(x => x.Uplate)
                .Include(x => x.StavkeTermina).ThenInclude(x => x.Usluga)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.TerminId == id, cancellationToken)
                ?? throw new NotFoundException("Termin ne postoji.");

            TerminStateMachine.ProvjeriPrelaz(termin.Status, noviStatus);
            poslovnaPravila(termin);

            int korisnikId = _currentUser.KorisnikId;
            termin.Status = noviStatus;
            termin.DatumPromjeneStatusa = DateTime.UtcNow;
            termin.StatusPromijenio = await Context.Korisnici.FirstAsync(x => x.KorisnikId == korisnikId, cancellationToken);
            termin.RazlogPromjeneStatusa = razlog;

            await Context.SaveChangesAsync(cancellationToken);

            return Mapper.Map<Model.Termini>(termin);
        }

        private void ProvjeriDatumIVrijeme(DateOnly datum, TimeOnly pocetak)
        {
            var sada = LokalnoSada;

            if (datum.ToDateTime(pocetak) <= sada)
            {
                throw new BusinessException("Termin mora biti zakazan u budućnosti.");
            }

            if (datum > DateOnly.FromDateTime(sada).AddDays(_settings.MaksimalnoDanaUnaprijed))
            {
                throw new BusinessException($"Termin je moguće zakazati najviše {_settings.MaksimalnoDanaUnaprijed} dana unaprijed.");
            }

            if (pocetak.Second != 0 || (pocetak.Hour * 60 + pocetak.Minute) % _settings.IntervalMinuta != 0)
            {
                throw new BusinessException($"Vrijeme početka mora biti usklađeno s intervalom od {_settings.IntervalMinuta} minuta (npr. 09:00, 09:{_settings.IntervalMinuta:00}).");
            }
        }

        private TimeOnly IzracunajKraj(DateOnly datum, TimeOnly pocetak, int trajanje)
        {
            var pocetakDatum = datum.ToDateTime(pocetak);
            var krajDatum = pocetakDatum.AddMinutes(trajanje);

            if (pocetak < RadnoVrijemeOd || krajDatum.Date != pocetakDatum.Date || TimeOnly.FromDateTime(krajDatum) > RadnoVrijemeDo)
            {
                throw new BusinessException($"Termin mora biti unutar radnog vremena ({RadnoVrijemeOd:HH\\:mm} - {_settings.RadnoVrijemeDoSat:00}:00). Odabrane usluge ukupno traju {trajanje} minuta.");
            }

            return TimeOnly.FromDateTime(krajDatum);
        }

        private async Task<AktiviranaPromocija?> UcitajPromocijuAsync(string? kod, int klijentId, List<Usluga> usluge, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(kod))
            {
                return null;
            }

            var normalizovaniKod = kod.Trim().ToUpperInvariant();

            var aktivirana = await Context.AktiviranePromocije
                .Include(x => x.Promocija).ThenInclude(x => x.Usluga)
                .FirstOrDefaultAsync(x => x.KorisnikId == klijentId && x.Promocija.Kod == normalizovaniKod, cancellationToken)
                ?? throw new BusinessException("Kupon nije aktiviran. Promociju najprije aktivirajte u sekciji Promocije.");

            if (aktivirana.Iskoristena)
            {
                throw new BusinessException("Ovaj kupon je već iskorišten.");
            }

            var sada = DateTime.UtcNow;
            var promocija = aktivirana.Promocija;
            if (!promocija.JeAktivna || promocija.DatumPocetka > sada || promocija.DatumKraja < sada)
            {
                throw new BusinessException("Kupon je istekao ili trenutno nije važeći.");
            }

            if (usluge.All(x => x.UslugaId != promocija.UslugaId))
            {
                throw new BusinessException($"Kupon se odnosi na uslugu '{promocija.Usluga.Naziv}' koja nije u korpi.");
            }

            return aktivirana;
        }

        // Cijene se uvijek računaju na serveru iz kataloga usluga; klijent ne šalje iznose.
        private void IzracunajCijenu(Termin termin, List<Usluga> usluge, AktiviranaPromocija? aktiviranaPromocija)
        {
            decimal iznosUsluga = usluge.Sum(x => x.Cijena);
            decimal popust = 0;

            if (aktiviranaPromocija != null)
            {
                var usluga = usluge.First(x => x.UslugaId == aktiviranaPromocija.Promocija.UslugaId);
                popust = Math.Round(usluga.Cijena * aktiviranaPromocija.Promocija.Popust / 100m, 2, MidpointRounding.AwayFromZero);
                popust = Math.Min(popust, usluga.Cijena);
            }

            termin.IznosPopusta = popust;
            termin.CijenaPrevoza = termin.JePrevoz ? _settings.CijenaPrevoza : 0;
            termin.UkupnaCijena = Math.Max(iznosUsluga - popust + termin.CijenaPrevoza, 0);
        }

        private async Task ProvjeriZauzetostAsync(Termin termin, CancellationToken cancellationToken)
        {
            var preklapajuci = Context.Termini.Where(x => x.DatumTermina == termin.DatumTermina
                && TerminStateMachine.AktivniStatusi.Contains(x.Status)
                && x.VrijemePocetka < termin.VrijemeKraja
                && x.VrijemeKraja > termin.VrijemePocetka);

            if (await preklapajuci.AnyAsync(x => x.VeterinarId == termin.Veterinar.KorisnikId, cancellationToken))
            {
                throw new BusinessException("Veterinar je zauzet u odabranom terminu. Odaberite drugo vrijeme.");
            }

            if (await preklapajuci.AnyAsync(x => x.LjubimacId == termin.Ljubimac.LjubimacId, cancellationToken))
            {
                throw new BusinessException("Ljubimac već ima zakazan termin koji se preklapa s odabranim vremenom.");
            }
        }

        private static void ProvjeriDaNijePlacen(Termin termin, string akcija)
        {
            if (termin.Uplate.Any(x => x.Status == StatusUplate.Placeno))
            {
                throw new BusinessException($"Plaćeni termin nije moguće {akcija} bez povrata novca. Pokrenite povrat (refund) uplate.");
            }
        }

        private static void VratiPromociju(Termin termin)
        {
            if (termin.AktiviranaPromocija != null)
            {
                termin.AktiviranaPromocija.Iskoristena = false;
            }
        }

        private IQueryable<Korisnik> AktivniVeterinari()
        {
            return Context.Korisnici.Where(x => x.JeAktivan && x.KorisniciUloge.Any(ku => ku.Uloga.Naziv == Uloge.Veterinar));
        }

        private static string GenerisiSifru()
        {
            return "T-" + RandomNumberGenerator.GetString(SifraZnakovi, SifraDuzina);
        }

        private static string Formatiraj(Termin termin)
        {
            return $"{termin.DatumTermina:dd.MM.yyyy.} u {termin.VrijemePocetka:HH\\:mm}";
        }
    }
}
