using eVet.Model.Exceptions;
using eVet.Services.Security;
using Microsoft.AspNetCore.Http;

namespace eVet.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int KorisnikId
        {
            get
            {
                var vrijednost = _httpContextAccessor.HttpContext?.User.FindFirst(JwtClaimNames.KorisnikId)?.Value;

                return int.TryParse(vrijednost, out var id)
                    ? id
                    : throw new UnauthorizedException("Niste prijavljeni.");
            }
        }

        public bool IsInRole(string uloga)
        {
            return _httpContextAccessor.HttpContext?.User.IsInRole(uloga) == true;
        }
    }
}
