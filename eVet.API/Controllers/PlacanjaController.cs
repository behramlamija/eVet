using eVet.Model;
using eVet.Model.Constants;
using eVet.Services.Placanja;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = Uloge.Klijent)]
    public class PlacanjaController : ControllerBase
    {
        private readonly IPlacanjaService _placanjaService;

        public PlacanjaController(IPlacanjaService placanjaService)
        {
            _placanjaService = placanjaService;
        }

        /// <summary>Otvara PayPal narudžbu za termin; iznos određuje server na osnovu termina.</summary>
        [HttpPost("termin/{terminId}/paypal")]
        public Task<PayPalNarudzba> ZapocniPayPal(int terminId, CancellationToken cancellationToken)
        {
            return _placanjaService.ZapocniPayPalAsync(terminId, cancellationToken);
        }

        /// <summary>Naplata i potvrda uplate na serveru nakon povratka iz PayPala.</summary>
        [HttpPost("{uplataId}/potvrdi")]
        public Task<Termini> PotvrdiPayPal(int uplataId, CancellationToken cancellationToken)
        {
            return _placanjaService.PotvrdiPayPalAsync(uplataId, cancellationToken);
        }
    }
}
