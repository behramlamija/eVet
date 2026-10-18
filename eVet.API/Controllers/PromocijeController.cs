using eVet.Model;
using eVet.Model.Constants;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    public class PromocijeController : VlasnikCRUDController<Promocije, PromocijeSearchObject, PromocijeUpsertRequest, PromocijeUpsertRequest>
    {
        private readonly IPromocijeService _promocijeService;

        public PromocijeController(IPromocijeService service) : base(service)
        {
            _promocijeService = service;
        }

        [Authorize(Roles = Uloge.Klijent)]
        [HttpPost("{id}/aktiviraj")]
        public Task<AktiviranePromocije> Aktiviraj(int id, CancellationToken cancellationToken)
        {
            return _promocijeService.AktivirajAsync(id, cancellationToken);
        }

        [Authorize(Roles = Uloge.Klijent)]
        [HttpGet("moje")]
        public Task<PagedResult<AktiviranePromocije>> GetMoje([FromQuery] BaseSearchObject search, CancellationToken cancellationToken)
        {
            return _promocijeService.GetMojeAktiviraneAsync(search, cancellationToken);
        }
    }
}
