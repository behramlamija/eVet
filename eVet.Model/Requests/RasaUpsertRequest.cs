using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class RasaUpsertRequest
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(100, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Naziv { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = ValidacijskaPravila.OdabirPoruka)]
        public int VrstaLjubimcaId { get; set; }
    }
}
