using eVet.Model;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PreporukeController : ControllerBase
    {
        private readonly IPreporukeService _preporukeService;

        public PreporukeController(IPreporukeService preporukeService)
        {
            _preporukeService = preporukeService;
        }

        /// <summary>
        /// "Korisnici također rezervišu": usluge koje se najčešće rezervišu zajedno s navedenim uslugama
        /// (jedna usluga na detaljima usluge ili sve usluge iz korpe).
        /// </summary>
        [HttpGet]
        public Task<List<PreporukeUsluga>> Get([FromQuery] List<int> uslugeIds, CancellationToken cancellationToken)
        {
            return _preporukeService.GetPreporukeAsync(uslugeIds, cancellationToken);
        }
    }
}
