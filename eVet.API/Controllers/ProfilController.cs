using eVet.Model;
using eVet.Model.Requests;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProfilController : ControllerBase
    {
        private readonly IKorisniciService _korisniciService;

        public ProfilController(IKorisniciService korisniciService)
        {
            _korisniciService = korisniciService;
        }

        [HttpGet]
        public Task<Korisnici> Get(CancellationToken cancellationToken)
        {
            return _korisniciService.GetProfilAsync(cancellationToken);
        }

        [HttpPut]
        public Task<Korisnici> Update(ProfilUpdateRequest request, CancellationToken cancellationToken)
        {
            return _korisniciService.UpdateProfilAsync(request, cancellationToken);
        }

        [HttpPut("lozinka")]
        public async Task<IActionResult> PromijeniLozinku(PromjenaLozinkeRequest request, CancellationToken cancellationToken)
        {
            await _korisniciService.PromijeniLozinkuAsync(request, cancellationToken);
            return NoContent();
        }
    }
}
