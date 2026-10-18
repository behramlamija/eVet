using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class VrstaUslugeUpsertRequest
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(100, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Naziv { get; set; } = null!;

        public bool JeOperacija { get; set; }

        public bool JeVakcinacija { get; set; }
    }
}
