using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class KorisniciUpdateRequest : IValidatableObject
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(100, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Ime { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(100, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Prezime { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [EmailAddress(ErrorMessage = ValidacijskaPravila.EmailPoruka)]
        [MaxLength(255, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Email { get; set; } = null!;

        [RegularExpression(ValidacijskaPravila.TelefonRegex, ErrorMessage = ValidacijskaPravila.TelefonPoruka)]
        public string? Telefon { get; set; }

        public bool JeAktivan { get; set; } = true;

        [MinLength(1, ErrorMessage = "Korisniku je potrebno dodijeliti barem jednu ulogu.")]
        public List<int> UlogeIds { get; set; } = new List<int>();

        // Popunjava se samo kada administrator mijenja lozinku korisniku; stara lozinka nije potrebna.
        [RegularExpression(ValidacijskaPravila.LozinkaRegex, ErrorMessage = ValidacijskaPravila.LozinkaPoruka)]
        public string? Lozinka { get; set; }

        [Compare(nameof(Lozinka), ErrorMessage = ValidacijskaPravila.LozinkePoklapanjePoruka)]
        public string? LozinkaPotvrda { get; set; }

        public DateOnly? DatumRodjenja { get; set; }

        public int? GradRodjenjaId { get; set; }

        public DateOnly? DatumZaposlenja { get; set; }

        [MaxLength(100, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string? Zvanje { get; set; }

        public int? SpecijalnostId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ValidacijaPodatakaZaposlenja.Validiraj(DatumRodjenja, DatumZaposlenja);
        }
    }
}
