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

        public AuthService(EVetDbContext context, IMapper mapper, ICurrentUserService currentUser, IMemoryCache cache, IOptions<JwtSettings> jwtSettings, ILogger<AuthService> logger)
        {
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

            if (korisnik == null || !PasswordHasher.Verify(request.Lozinka, korisnik.LozinkaSalt, korisnik.LozinkaHash))
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
            korisnik.SecurityStamp = Guid.NewGuid().ToString();
            await _context.SaveChangesAsync(cancellationToken);

            SecurityStampCache.Ponisti(_cache, korisnikId);
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
