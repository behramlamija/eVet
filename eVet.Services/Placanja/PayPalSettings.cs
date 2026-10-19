using System.ComponentModel.DataAnnotations;

namespace eVet.Services.Placanja
{
    // Vrijednosti se čitaju iz .env datoteke (PayPal__*).
    public class PayPalSettings
    {
        public const string SectionName = "PayPal";

        [Required, Url]
        public string BaseUrl { get; set; } = null!;

        [Required]
        public string ClientId { get; set; } = null!;

        [Required]
        public string ClientSecret { get; set; } = null!;

        // PayPal ne podržava KM (BAM), pa se cijena termina naplaćuje u ovoj valuti po fiksnom kursu.
        [Required, StringLength(3, MinimumLength = 3)]
        public string Valuta { get; set; } = null!;

        [Range(0.0001, 1000)]
        public decimal KursKonverzije { get; set; }

        // Adrese na koje PayPal vraća kupca; mobilna aplikacija ih presreće u ugrađenom pregledniku.
        [Required, Url]
        public string ReturnUrl { get; set; } = null!;

        [Required, Url]
        public string CancelUrl { get; set; } = null!;
    }
}
