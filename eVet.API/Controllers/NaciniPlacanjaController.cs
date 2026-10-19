using eVet.Model;
using eVet.Model.Constants;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    public class NaciniPlacanjaController : BaseController<NacinPlacanja, NazivSearchObject>
    {
        private readonly INaciniPlacanjaService _naciniPlacanjaService;

        public NaciniPlacanjaController(INaciniPlacanjaService service) : base(service)
        {
            _naciniPlacanjaService = service;
        }

        [Authorize(Roles = Uloge.Vlasnik)]
        [HttpPut("{id}")]
        public Task<NacinPlacanja> Update(int id, NazivUpsertRequest request, CancellationToken cancellationToken)
        {
            return _naciniPlacanjaService.UpdateAsync(id, request, cancellationToken);
        }
    }
}
