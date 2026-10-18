namespace eVet.Model
{
    public class Usluge
    {
        public int UslugaId { get; set; }

        public string Naziv { get; set; } = null!;

        public string? Opis { get; set; }

        public decimal Cijena { get; set; }

        public int Trajanje { get; set; }

        public int VrstaId { get; set; }

        public string VrstaNaziv { get; set; } = null!;

        public DateTime DatumObjavljivanja { get; set; }

        public bool ImaSliku { get; set; }
    }
}
