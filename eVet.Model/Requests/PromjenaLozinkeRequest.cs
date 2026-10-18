using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class PromjenaLozinkeRequest
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        public string StaraLozinka { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [RegularExpression(ValidacijskaPravila.LozinkaRegex, ErrorMessage = ValidacijskaPravila.LozinkaPoruka)]
        public string NovaLozinka { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [Compare(nameof(NovaLozinka), ErrorMessage = ValidacijskaPravila.LozinkePoklapanjePoruka)]
        public string NovaLozinkaPotvrda { get; set; } = null!;
    }
}
