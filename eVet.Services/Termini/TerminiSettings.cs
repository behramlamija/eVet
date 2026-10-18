using System.ComponentModel.DataAnnotations;

namespace eVet.Services.Termini
{
    // Vrijednosti se čitaju iz .env datoteke (Termini__*).
    public class TerminiSettings
    {
        public const string SectionName = "Termini";

        [Range(0, 23)]
        public int RadnoVrijemeOdSat { get; set; }

        [Range(1, 24)]
        public int RadnoVrijemeDoSat { get; set; }

        [Range(5, 120)]
        public int IntervalMinuta { get; set; }

        [Range(0, 1000)]
        public decimal CijenaPrevoza { get; set; }

        [Range(0, 168)]
        public int MinimalnoSatiZaOtkazivanje { get; set; }

        [Range(1, 365)]
        public int MaksimalnoDanaUnaprijed { get; set; }

        [Required]
        public string VremenskaZona { get; set; } = null!;
    }
}
