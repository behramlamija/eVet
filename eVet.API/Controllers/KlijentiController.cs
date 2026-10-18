using eVet.Model;
using eVet.Model.Constants;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    // Lista klijenata za odabir vlasnika pri dodavanju pacijenta.
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = Uloge.Veterinar + "," + Uloge.Vlasnik)]
    public class KlijentiController : ControllerBase
    {
        private readonly IKorisniciService _korisniciService;

        public KlijentiController(IKorisniciService korisniciService)
        {
            _korisniciService = korisniciService;
        }

        [HttpGet]
        public Task<PagedResult<KorisnikOsnovno>> GetAll([FromQuery] KorisnikOsnovnoSearchObject search, CancellationToken cancellationToken)
        {
            return _korisniciService.GetPoUloziAsync(Uloge.Klijent, search, cancellationToken);
        }
    }
}
