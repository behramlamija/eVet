namespace eVet.Model
{
    // Pravila zakazivanja iz konfiguracije servera, za prikaz u klijentskoj aplikaciji.
    public class TerminiKonfiguracija
    {
        public decimal CijenaPrevoza { get; set; }

        public int MinimalnoSatiZaOtkazivanje { get; set; }

        public int MaksimalnoDanaUnaprijed { get; set; }

        public int RadnoVrijemeOdSat { get; set; }

        public int RadnoVrijemeDoSat { get; set; }
    }
}
