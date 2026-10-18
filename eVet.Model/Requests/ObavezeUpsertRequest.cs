using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class ObavezeUpsertRequest
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(255, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Naslov { get; set; } = null!;

        [MaxLength(1000, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string? Opis { get; set; }

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        public DateTime? DatumVrijeme { get; set; }

        public bool JeZavrsena { get; set; }
    }
}
