using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.Responses;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IKorisniciService _korisniciService;

        public AuthController(IAuthService authService, IKorisniciService korisniciService)
        {
            _authService = authService;
            _korisniciService = korisniciService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            return _authService.LoginAsync(request, cancellationToken);
        }

        [AllowAnonymous]
        [HttpPost("registracija")]
        public Task<Korisnici> Registracija(RegistracijaRequest request, CancellationToken cancellationToken)
        {
            return _korisniciService.RegistracijaAsync(request, cancellationToken);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            await _authService.LogoutAsync(cancellationToken);
            return NoContent();
        }
    }
}
