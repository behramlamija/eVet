namespace eVet.Model.Statistika
{
    public class StatistikaVeterinara
    {
        public int KorisnikId { get; set; }

        public string Ime { get; set; } = null!;

        public string Prezime { get; set; } = null!;

        public bool ImaSliku { get; set; }

        public string? SpecijalnostNaziv { get; set; }

        public bool JeAktivan { get; set; }

        public int BrojTermina { get; set; }

        public int BrojZavrsenihTermina { get; set; }

        public int PozitivneRecenzije { get; set; }

        public int NegativneRecenzije { get; set; }
    }
}
