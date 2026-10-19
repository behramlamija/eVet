using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    // Naziv uloge se ne mijenja jer ga koristi autorizacija ([Authorize(Roles = ...)]).
    public class UlogaUpdateRequest
    {
        [MaxLength(255, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string? Opis { get; set; }
    }
}
