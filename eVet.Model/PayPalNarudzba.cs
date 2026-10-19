namespace eVet.Model
{
    /// <summary>
    /// Otvorena PayPal narudžba za termin. Klijent otvara <see cref="ApproveUrl"/> u ugrađenom pregledniku,
    /// a povratak na <see cref="ReturnUrl"/> ili <see cref="CancelUrl"/> označava kraj PayPal koraka.
    /// </summary>
    public class PayPalNarudzba
    {
        public int UplataId { get; set; }

        public string OrderId { get; set; } = null!;

        public string ApproveUrl { get; set; } = null!;

        public string ReturnUrl { get; set; } = null!;

        public string CancelUrl { get; set; } = null!;

        public decimal Iznos { get; set; }

        public string Valuta { get; set; } = null!;
    }
}
