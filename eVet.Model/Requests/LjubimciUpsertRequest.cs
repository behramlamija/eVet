using eVet.Model.Constants;
using eVet.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class LjubimciUpsertRequest : IValidatableObject
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(100, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Ime { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = ValidacijskaPravila.OdabirPoruka)]
        public int RasaId { get; set; }

        public DateOnly? DatumRodjenja { get; set; }

        [EnumDataType(typeof(Spol), ErrorMessage = "Odaberite spol ljubimca.")]
        public Spol Spol { get; set; }

        [Range(typeof(decimal), "0.01", "200", ErrorMessage = "Težina mora biti između 0,01 i 200 kg.")]
        public decimal? Tezina { get; set; }

        [MaxLength(1000, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string? Napomena { get; set; }

        // Koristi samo osoblje stanice kada dodaje pacijenta; klijentu se vlasnik uvijek uzima iz tokena.
        public int? VlasnikId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DatumRodjenja.HasValue && DatumRodjenja.Value > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                yield return new ValidationResult("Datum rođenja ne može biti u budućnosti.", new[] { nameof(DatumRodjenja) });
            }
        }
    }
}
