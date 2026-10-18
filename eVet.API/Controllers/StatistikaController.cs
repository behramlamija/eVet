using eVet.Model;
using eVet.Model.Constants;
using eVet.Model.Enums;
using eVet.Model.SearchObjects;
using eVet.Model.Statistika;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = Uloge.Vlasnik)]
    public class StatistikaController : ControllerBase
    {
        private readonly IStatistikaService _statistikaService;

        public StatistikaController(IStatistikaService statistikaService)
        {
            _statistikaService = statistikaService;
        }

        [HttpGet("poslovanje")]
        public Task<StatistikaPoslovanja> GetPoslovanje([FromQuery] DateOnly? datumOd, [FromQuery] DateOnly? datumDo, CancellationToken cancellationToken)
        {
            return _statistikaService.GetPoslovanjeAsync(datumOd, datumDo, cancellationToken);
        }

        [HttpGet("po-periodima")]
        public Task<List<StatistikaPeriod>> GetPoPeriodima([FromQuery] GrupisanjeStatistike grupisanje, [FromQuery] int? godina, CancellationToken cancellationToken)
        {
            return _statistikaService.GetPoPeriodimaAsync(grupisanje, godina, cancellationToken);
        }

        [HttpGet("veterinari")]
        public Task<PagedResult<StatistikaVeterinara>> GetVeterinari([FromQuery] KorisnikOsnovnoSearchObject search, CancellationToken cancellationToken)
        {
            return _statistikaService.GetVeterinareAsync(search, cancellationToken);
        }
    }
}
