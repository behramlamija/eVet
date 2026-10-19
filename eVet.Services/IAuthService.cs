using eVet.Model.Requests;
using eVet.Model.Responses;

namespace eVet.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

        Task LogoutAsync(CancellationToken cancellationToken = default);

        Task ZatraziResetLozinkeAsync(ZaboravljenaLozinkaRequest request, CancellationToken cancellationToken = default);

        Task ResetujLozinkuAsync(ResetLozinkeRequest request, CancellationToken cancellationToken = default);

        Task<bool> IsSecurityStampValidAsync(int korisnikId, string securityStamp, CancellationToken cancellationToken = default);
    }
}
