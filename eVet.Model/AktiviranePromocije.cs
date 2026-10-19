namespace eVet.Model
{
    public class AktiviranePromocije
    {
        public int AktiviranaPromocijaId { get; set; }

        public int PromocijaId { get; set; }

        public string PromocijaNaziv { get; set; } = null!;

        public string PromocijaKod { get; set; } = null!;

        public decimal PromocijaPopust { get; set; }

        public DateTime PromocijaDatumKraja { get; set; }

        public int UslugaId { get; set; }

        public string UslugaNaziv { get; set; } = null!;

        public bool Iskoristena { get; set; }

        public DateTime DatumAktiviranja { get; set; }
    }
}
