namespace eVet.Model
{
    public class Promocije
    {
        public int PromocijaId { get; set; }

        public string Naziv { get; set; } = null!;

        public string? Opis { get; set; }

        public string Kod { get; set; } = null!;

        public decimal Popust { get; set; }

        public DateTime DatumPocetka { get; set; }

        public DateTime DatumKraja { get; set; }

        public int UslugaId { get; set; }

        public string UslugaNaziv { get; set; } = null!;

        public decimal UslugaCijena { get; set; }

        public bool JeAktivna { get; set; }
    }
}
