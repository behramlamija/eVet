using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class NazivUpsertRequest
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(100, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Naziv { get; set; } = null!;
    }
}
