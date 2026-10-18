using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class RecenzijaInsertRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Potrebno je odabrati termin koji recenzirate.")]
        public int TerminId { get; set; }

        public bool UslugaPozitivna { get; set; }

        public bool VeterinarPozitivan { get; set; }

        [MaxLength(1000, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string? Komentar { get; set; }
    }
}
