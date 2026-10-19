namespace eVet.Model
{
    /// <summary>
    /// Usluga preporučena na osnovu toga koliko se često rezerviše zajedno s polaznim uslugama,
    /// uz objašnjenje zašto se preporučuje.
    /// </summary>
    public class PreporukeUsluga
    {
        public Usluge Usluga { get; set; } = null!;

        public int UslugaIzvorId { get; set; }

        public string UslugaIzvorNaziv { get; set; } = null!;

        public int BrojZajednickihTermina { get; set; }

        public int BrojTerminaIzvora { get; set; }

        public decimal Pouzdanost { get; set; }

        public string Objasnjenje { get; set; } = null!;
    }
}
