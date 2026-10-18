using eVet.Model.Enums;
using eVet.Services.Slike;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    // Slike se ne šalju u listama kao base64, nego se dohvaćaju preko ovog endpointa (uz provjeru prava pristupa).
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SlikeController : ControllerBase
    {
        private readonly ISlikeService _slikeService;

        public SlikeController(ISlikeService slikeService)
        {
            _slikeService = slikeService;
        }

        [HttpGet("{tip}/{id}")]
        public async Task<IActionResult> Get(TipSlike tip, int id, CancellationToken cancellationToken)
        {
            var slika = await _slikeService.GetAsync(tip, id, cancellationToken);
            return File(slika.Sadrzaj, slika.ContentType);
        }

        [HttpPut("{tip}/{id}")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(SlikaValidator.MaksimalnaVelicina + 64 * 1024)]
        public async Task<IActionResult> Set(TipSlike tip, int id, IFormFile slika, CancellationToken cancellationToken)
        {
            await using var stream = slika.OpenReadStream();
            using var memory = new MemoryStream();
            await stream.CopyToAsync(memory, cancellationToken);

            await _slikeService.SetAsync(tip, id, memory.ToArray(), slika.ContentType, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{tip}/{id}")]
        public async Task<IActionResult> Delete(TipSlike tip, int id, CancellationToken cancellationToken)
        {
            await _slikeService.DeleteAsync(tip, id, cancellationToken);
            return NoContent();
        }
    }
}
