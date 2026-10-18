using eVet.Model;
using eVet.Model.Constants;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    // Lista aktivnih veterinara za odabir pri zakazivanju termina.
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class VeterinariController : ControllerBase
    {
        private readonly IKorisniciService _korisniciService;

        public VeterinariController(IKorisniciService korisniciService)
        {
            _korisniciService = korisniciService;
        }

        [HttpGet]
        public Task<PagedResult<KorisnikOsnovno>> GetAll([FromQuery] KorisnikOsnovnoSearchObject search, CancellationToken cancellationToken)
        {
            return _korisniciService.GetPoUloziAsync(Uloge.Veterinar, search, cancellationToken);
        }
    }
}
