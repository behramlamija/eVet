using eVet.Model.Constants;
using System.ComponentModel.DataAnnotations;

namespace eVet.Model.Requests
{
    public class TerapijeUpsertRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = ValidacijskaPravila.OdabirPoruka)]
        public int LjubimacId { get; set; }

        public int? TerminId { get; set; }

        [Required(ErrorMessage = ValidacijskaPravila.ObaveznoPoljePoruka)]
        [MaxLength(500, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string Dijagnoza { get; set; } = null!;

        [MaxLength(255, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string? Lijek { get; set; }

        [MaxLength(100, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string? Doza { get; set; }

        [MaxLength(1000, ErrorMessage = ValidacijskaPravila.MaksimalnaDuzinaPoruka)]
        public string? Napomena { get; set; }

        [Range(1, 365, ErrorMessage = "Trajanje terapije mora biti između 1 i 365 dana.")]
        public int? TrajanjeDana { get; set; }

        public DateOnly? DatumKontrolnog { get; set; }
    }
}
