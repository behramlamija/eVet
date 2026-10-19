using eVet.Model;
using eVet.Model.Constants;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    [Authorize(Roles = Uloge.Vlasnik)]
    public class UlogeController : BaseController<Uloga, NazivSearchObject>
    {
        private readonly IUlogeService _ulogeService;

        public UlogeController(IUlogeService service) : base(service)
        {
            _ulogeService = service;
        }

        [HttpPut("{id}")]
        public Task<Uloga> Update(int id, UlogaUpdateRequest request, CancellationToken cancellationToken)
        {
            return _ulogeService.UpdateAsync(id, request, cancellationToken);
        }
    }
}
