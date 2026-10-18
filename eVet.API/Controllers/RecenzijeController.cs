using eVet.Model;
using eVet.Model.Constants;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    public class RecenzijeController : BaseController<Recenzije, RecenzijeSearchObject>
    {
        private readonly IRecenzijeService _recenzijeService;

        public RecenzijeController(IRecenzijeService service) : base(service)
        {
            _recenzijeService = service;
        }

        [Authorize(Roles = Uloge.Klijent)]
        [HttpPost]
        public Task<Recenzije> Insert(RecenzijaInsertRequest request, CancellationToken cancellationToken)
        {
            return _recenzijeService.InsertAsync(request, cancellationToken);
        }

        [Authorize(Roles = Uloge.Veterinar + "," + Uloge.Vlasnik)]
        [HttpPost("{id}/odgovori")]
        public Task<Recenzije> Odgovori(int id, RecenzijaOdgovorRequest request, CancellationToken cancellationToken)
        {
            return _recenzijeService.OdgovoriAsync(id, request, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _recenzijeService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
