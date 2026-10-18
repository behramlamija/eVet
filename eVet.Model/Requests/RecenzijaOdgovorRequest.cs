using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class RecenzijaOdgovorRequest
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(1000, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Komentar { get; set; } = null!;
    }
}
