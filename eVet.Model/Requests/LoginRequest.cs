using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        public string KorisnickoIme { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        public string Lozinka { get; set; } = null!;
    }
}
