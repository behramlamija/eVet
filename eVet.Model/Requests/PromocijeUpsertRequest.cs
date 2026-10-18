using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class PromocijeUpsertRequest : IValidatableObject
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(255, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Naziv { get; set; } = null!;

        [MaxLength(1000, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string? Opis { get; set; }

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [RegularExpression(ValidacijskaPravila.KodPromocijeRegex, ErrorMessage = ValidacijskaPravila.KodPromocijePoruka)]
        public string Kod { get; set; } = null!;

        [Range(typeof(decimal), "1", "100", ErrorMessage = "Popust mora biti između 1 i 100 %.")]
        public decimal Popust { get; set; }

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        public DateTime? DatumPocetka { get; set; }

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        public DateTime? DatumKraja { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ValidacijskaPravila.OdabirPoruka)]
        public int UslugaId { get; set; }

        public bool JeAktivna { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DatumPocetka.HasValue && DatumKraja.HasValue && DatumKraja <= DatumPocetka)
            {
                yield return new ValidationResult("Datum kraja promocije mora biti nakon datuma početka.", new[] { nameof(DatumKraja) });
            }
        }
    }
}
