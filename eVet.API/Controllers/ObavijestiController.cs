using eVet.Model;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    public class ObavijestiController : BaseController<Obavijesti, ObavijestiSearchObject>
    {
        private readonly IObavijestiService _obavijestiService;

        public ObavijestiController(IObavijestiService service) : base(service)
        {
            _obavijestiService = service;
        }

        [HttpGet("broj-neprocitanih")]
        public Task<int> GetBrojNeprocitanih(CancellationToken cancellationToken)
        {
            return _obavijestiService.GetBrojNeprocitanihAsync(cancellationToken);
        }

        [HttpPut("{id}/procitano")]
        public async Task<IActionResult> OznaciProcitano(int id, CancellationToken cancellationToken)
        {
            await _obavijestiService.OznaciProcitanoAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPut("procitaj-sve")]
        public async Task<IActionResult> OznaciSveProcitano(CancellationToken cancellationToken)
        {
            await _obavijestiService.OznaciSveProcitanoAsync(cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _obavijestiService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
