using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class ZaboravljenaLozinkaRequest
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [EmailAddress(ErrorMessage = ValidacijskaPravila.EmailPoruka)]
        public string Email { get; set; } = null!;
    }
}
