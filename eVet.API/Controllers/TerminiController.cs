using eVet.Model;
using eVet.Model.Constants;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Termini;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    // Termini se ne brišu niti uređuju direktno; mijenja im se samo status kroz state machine.
    public class TerminiController : BaseController<Termini, TerminiSearchObject>
    {
        private const string Osoblje = Uloge.Veterinar + "," + Uloge.Vlasnik;

        private readonly ITerminiService _terminiService;

        public TerminiController(ITerminiService service) : base(service)
        {
            _terminiService = service;
        }

        [Authorize(Roles = Uloge.Klijent)]
        [HttpPost]
        public Task<Termini> Kreiraj(TerminInsertRequest request, CancellationToken cancellationToken)
        {
            return _terminiService.KreirajAsync(request, cancellationToken);
        }

        [Authorize(Roles = Osoblje)]
        [HttpPut("{id}/potvrdi")]
        public Task<Termini> Potvrdi(int id, CancellationToken cancellationToken)
        {
            return _terminiService.PotvrdiAsync(id, cancellationToken);
        }

        [Authorize(Roles = Osoblje)]
        [HttpPut("{id}/odbij")]
        public Task<Termini> Odbij(int id, PromjenaStatusaRequest request, CancellationToken cancellationToken)
        {
            return _terminiService.OdbijAsync(id, request, cancellationToken);
        }

        [HttpPut("{id}/otkazi")]
        public Task<Termini> Otkazi(int id, PromjenaStatusaRequest request, CancellationToken cancellationToken)
        {
            return _terminiService.OtkaziAsync(id, request, cancellationToken);
        }

        [Authorize(Roles = Osoblje)]
        [HttpPut("{id}/zavrsi")]
        public Task<Termini> Zavrsi(int id, CancellationToken cancellationToken)
        {
            return _terminiService.ZavrsiAsync(id, cancellationToken);
        }

        [HttpGet("slobodna-vremena")]
        public Task<List<TimeOnly>> GetSlobodnaVremena([FromQuery] int veterinarId, [FromQuery] DateOnly datum, [FromQuery] int trajanje, CancellationToken cancellationToken)
        {
            return _terminiService.GetSlobodnaVremenaAsync(veterinarId, datum, trajanje, cancellationToken);
        }
    }
}
