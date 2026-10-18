using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class UslugeUpsertRequest
    {
        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(255, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Naziv { get; set; } = null!;

        [MaxLength(2000, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string? Opis { get; set; }

        [Range(typeof(decimal), "0.01", "100000", ErrorMessage = "Cijena mora biti između 0,01 i 100000 KM.")]
        public decimal Cijena { get; set; }

        [Range(5, 480, ErrorMessage = "Trajanje mora biti između 5 i 480 minuta.")]
        public int Trajanje { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = ValidacijskaPravila.OdabirPoruka)]
        public int VrstaId { get; set; }
    }
}
