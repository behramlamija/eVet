using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class TerminInsertRequest : IValidatableObject
    {
        [Range(1, int.MaxValue, ErrorMessage = "Potrebno je odabrati veterinara.")]
        public int VeterinarId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Potrebno je odabrati ljubimca.")]
        public int LjubimacId { get; set; }

        [Required(ErrorMessage = "Potrebno je odabrati datum termina.")]
        public DateOnly? DatumTermina { get; set; }

        [Required(ErrorMessage = "Potrebno je odabrati vrijeme početka termina.")]
        public TimeOnly? VrijemePocetka { get; set; }

        [MinLength(1, ErrorMessage = "Korpa je prazna. Dodajte barem jednu uslugu.")]
        public List<int> UslugeIds { get; set; } = new List<int>();

        [Range(1, int.MaxValue, ErrorMessage = "Potrebno je odabrati način plaćanja.")]
        public int NacinPlacanjaId { get; set; }

        public bool JePrevoz { get; set; }

        [MaxLength(255, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string? AdresaPrevoza { get; set; }

        public int? GradPrevozaId { get; set; }

        [MaxLength(50, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string? KodPromocije { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (JePrevoz && string.IsNullOrWhiteSpace(AdresaPrevoza))
            {
                yield return new ValidationResult("Za uslugu prevoza potrebno je unijeti adresu preuzimanja ljubimca.", new[] { nameof(AdresaPrevoza) });
            }

            if (JePrevoz && !GradPrevozaId.HasValue)
            {
                yield return new ValidationResult("Za uslugu prevoza potrebno je odabrati grad.", new[] { nameof(GradPrevozaId) });
            }
        }
    }
}
