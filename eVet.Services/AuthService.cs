using eVet.Messaging;
using eVet.Model.Exceptions;
using eVet.Model.Requests;
using eVet.Model.Responses;
using eVet.Services.Database;
using eVet.Services.Security;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace eVet.Services
{
    public class AuthService : IAuthService
    {
        private readonly EVetDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly IMemoryCache _cache;
        private readonly ILogger<AuthService> _logger;
        private readonly JwtSettings _jwtSettings;
        private readonly ResetLozinkeSettings _resetSettings;
        private readonly IPorukePublisher _publisher;

        public AuthService(EVetDbContext context, IMapper mapper, ICurrentUserService currentUser, IMemoryCache cache, IOptions<JwtSettings> jwtSettings,
            IOptions<ResetLozinkeSettings> resetSettings, IPorukePublisher publisher, ILogger<AuthService> logger)
        {
            _resetSettings = resetSettings.Value;
            _publisher = publisher;
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
            _cache = cache;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var korisnik = await _context.Korisnici
                .AsNoTracking()
                .Include(k => k.KorisniciUloge).ThenInclude(ku => ku.Uloga)
                .FirstOrDefaultAsync(k => k.KorisnickoIme == request.KorisnickoIme, cancellationToken);

            if (korisnik == null || !korisnik.ProvjeriLozinku(request.Lozinka))
            {
                _logger.LogWarning("Neuspješna prijava za korisničko ime {KorisnickoIme}", request.KorisnickoIme);
                throw new UnauthorizedException("Pogrešno korisničko ime ili lozinka.");
            }

            if (!korisnik.JeAktivan)
            {
                throw new UnauthorizedException("Vaš korisnički račun je deaktiviran. Obratite se vlasniku veterinarske stanice.");
            }

            var datumIsteka = DateTime.UtcNow.AddMinutes(_jwtSettings.TrajanjeMinuta);

            return new LoginResponse
            {
                Token = KreirajToken(korisnik, datumIsteka),
                DatumIsteka = datumIsteka,
                Korisnik = _mapper.Map<Model.Korisnici>(korisnik)
            };
        }

        public async Task LogoutAsync(CancellationToken cancellationToken = default)
        {
            int korisnikId = _currentUser.KorisnikId;

            var korisnik = await _context.Korisnici.FirstOrDefaultAsync(k => k.KorisnikId == korisnikId, cancellationToken)
                ?? throw new NotFoundException("Korisnik ne postoji.");

            // Nova vrijednost poništava sve ranije izdane tokene ovog korisnika.
            korisnik.PonistiPrijave();
            await _context.SaveChangesAsync(cancellationToken);

            SecurityStampCache.Ponisti(_cache, korisnikId);
        }

        public async Task ZatraziResetLozinkeAsync(ZaboravljenaLozinkaRequest request, CancellationToken cancellationToken = default)
        {
            var korisnik = await _context.Korisnici.FirstOrDefaultAsync(k => k.Email == request.Email && k.JeAktivan, cancellationToken);

            // Odgovor je isti i kada e-mail ne postoji, kako se ne bi otkrivalo koji su e-mailovi registrovani.
            if (korisnik == null)
            {
                _logger.LogWarning("Zatražen reset lozinke za nepostojeći ili neaktivan e-mail {Email}", request.Email);
                return;
            }

            var raniji = await _context.ResetiLozinki.Where(r => r.KorisnikId == korisnik.KorisnikId && !r.JeIskoristen).ToListAsync(cancellationToken);
            raniji.ForEach(r => r.JeIskoristen = true);

            var kod = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
            var salt = PasswordHasher.GenerateSalt();
            var sada = DateTime.UtcNow;

            _context.ResetiLozinki.Add(new ResetLozinke
            {
                Korisnik = korisnik,
                KodSalt = salt,
                KodHash = PasswordHasher.Hash(kod, salt),
                DatumKreiranja = sada,
                DatumIsteka = sada.AddMinutes(_resetSettings.TrajanjeMinuta)
            });
            await _context.SaveChangesAsync(cancellationToken);

            await _publisher.PosaljiEmailAsync(new EmailPoruka(
                korisnik.Email,
                $"{korisnik.Ime} {korisnik.Prezime}",
                "Kod za promjenu lozinke",
                $"Vaš kod za promjenu lozinke je {kod}. Kod vrijedi {_resetSettings.TrajanjeMinuta} minuta. Ako niste zatražili promjenu lozinke, zanemarite ovu poruku."),
                cancellationToken);
        }

        public async Task ResetujLozinkuAsync(ResetLozinkeRequest request, CancellationToken cancellationToken = default)
        {
            const string neispravanKod = "Kod je neispravan ili je istekao. Zatražite novi kod.";
            var sada = DateTime.UtcNow;

            var reset = await _context.ResetiLozinki
                .Include(r => r.Korisnik)
                .Where(r => r.Korisnik.Email == request.Email && r.Korisnik.JeAktivan && !r.JeIskoristen && r.DatumIsteka > sada)
                .OrderByDescending(r => r.DatumKreiranja)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new BusinessException(neispravanKod);

            if (!PasswordHasher.Verify(request.Kod, reset.KodSalt, reset.KodHash))
            {
                reset.BrojPokusaja++;
                if (reset.BrojPokusaja >= _resetSettings.MaksimalnoPokusaja)
                {
                    reset.JeIskoristen = true;
                }
                await _context.SaveChangesAsync(cancellationToken);

                int preostalo = _resetSettings.MaksimalnoPokusaja - reset.BrojPokusaja;
                throw new BusinessException(preostalo > 0
                    ? $"Kod nije ispravan. Preostalo pokušaja: {preostalo}."
                    : "Kod nije ispravan i više se ne može koristiti. Zatražite novi kod.");
            }

            reset.JeIskoristen = true;
            reset.Korisnik.PostaviLozinku(request.NovaLozinka);
            reset.Korisnik.PonistiPrijave();
            await _context.SaveChangesAsync(cancellationToken);

            SecurityStampCache.Ponisti(_cache, reset.KorisnikId);
        }

        public async Task<bool> IsSecurityStampValidAsync(int korisnikId, string securityStamp, CancellationToken cancellationToken = default)
        {
            var trenutniStamp = await _cache.GetOrCreateAsync(SecurityStampCache.Kljuc(korisnikId), async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = SecurityStampCache.Trajanje;

                return await _context.Korisnici
                    .AsNoTracking()
                    .Where(k => k.KorisnikId == korisnikId && k.JeAktivan)
                    .Select(k => k.SecurityStamp)
                    .FirstOrDefaultAsync(cancellationToken);
            });

            return trenutniStamp != null && trenutniStamp == securityStamp;
        }

        private string KreirajToken(Korisnik korisnik, DateTime datumIsteka)
        {
            var claims = new List<Claim>
            {
                new(JwtClaimNames.KorisnikId, korisnik.KorisnikId.ToString()),
                new(JwtClaimNames.KorisnickoIme, korisnik.KorisnickoIme),
                new(JwtClaimNames.SecurityStamp, korisnik.SecurityStamp)
            };
            claims.AddRange(korisnik.KorisniciUloge.Select(ku => new Claim(JwtClaimNames.Uloga, ku.Uloga.Naziv)));

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = datumIsteka,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                    SecurityAlgorithms.HmacSha256)
            };

            return new JsonWebTokenHandler().CreateToken(descriptor);
        }
    }
}
