using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class RegistracijaRequest
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(100, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Ime { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(100, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Prezime { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [RegularExpression(ValidacijskaPravila.KorisnickoImeRegex, ErrorMessage = ValidacijskaPravila.KorisnickoImePoruka)]
        public string KorisnickoIme { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [EmailAddress(ErrorMessage = ValidacijskaPravila.EmailPoruka)]
        [MaxLength(255, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Email { get; set; } = null!;

        [RegularExpression(ValidacijskaPravila.TelefonRegex, ErrorMessage = ValidacijskaPravila.TelefonPoruka)]
        public string? Telefon { get; set; }

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [RegularExpression(ValidacijskaPravila.LozinkaRegex, ErrorMessage = ValidacijskaPravila.LozinkaPoruka)]
        public string Lozinka { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [Compare(nameof(Lozinka), ErrorMessage = ValidacijskaPravila.LozinkePoklapanjePoruka)]
        public string LozinkaPotvrda { get; set; } = null!;
    }
}
