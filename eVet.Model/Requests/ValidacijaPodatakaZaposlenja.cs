using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public static class ValidacijaPodatakaZaposlenja
    {
        public static IEnumerable<ValidationResult> Validiraj(DateOnly? datumRodjenja, DateOnly? datumZaposlenja)
        {
            var danas = DateOnly.FromDateTime(DateTime.UtcNow);

            if (datumRodjenja.HasValue && datumRodjenja > danas.AddYears(-18))
            {
                yield return new ValidationResult("Uposlenik mora imati najmanje 18 godina.", new[] { "DatumRodjenja" });
            }

            if (datumZaposlenja.HasValue && datumZaposlenja > danas)
            {
                yield return new ValidationResult("Datum zaposlenja ne može biti u budućnosti.", new[] { "DatumZaposlenja" });
            }

            if (datumRodjenja.HasValue && datumZaposlenja.HasValue && datumZaposlenja < datumRodjenja.Value.AddYears(18))
            {
                yield return new ValidationResult("Datum zaposlenja mora biti nakon punoljetstva uposlenika.", new[] { "DatumZaposlenja" });
            }
        }
    }
}
