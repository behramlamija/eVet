namespace eVet.Model
{
    public class Usluge
    {
        public int UslugaId { get; set; }

        public string Naziv { get; set; } = null!;

        public string? Opis { get; set; }

        public decimal Cijena { get; set; }

        public int Trajanje { get; set; }

  

    }
}
