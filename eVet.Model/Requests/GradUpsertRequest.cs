using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class GradUpsertRequest
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(100, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Naziv { get; set; } = null!;

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [RegularExpression(ValidacijskaPravila.PostanskiBrojRegex, ErrorMessage = ValidacijskaPravila.PostanskiBrojPoruka)]
        public string PostanskiBroj { get; set; } = null!;
    }
}
