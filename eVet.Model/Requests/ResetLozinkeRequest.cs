using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class ResetLozinkeRequest
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [EmailAddress(ErrorMessage = ValidacijskaPravila.EmailPoruka)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Kod se sastoji od tačno 6 cifara, npr. 123456.")]
        public string Kod { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [RegularExpression(ValidacijskaPravila.LozinkaRegex, ErrorMessage = ValidacijskaPravila.LozinkaPoruka)]
        public string NovaLozinka { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [Compare(nameof(NovaLozinka), ErrorMessage = ValidacijskaPravila.LozinkePoklapanjePoruka)]
        public string NovaLozinkaPotvrda { get; set; } = null!;
    }
}
