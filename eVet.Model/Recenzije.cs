namespace eVet.Model
{
    public class Recenzije
    {
        public int RecenzijaId { get; set; }

        public int TerminId { get; set; }

        public string TerminSifra { get; set; } = null!;

        public int KorisnikId { get; set; }

        public string KorisnikIme { get; set; } = null!;

        public string KorisnikPrezime { get; set; } = null!;

        public int VeterinarId { get; set; }

        public string VeterinarIme { get; set; } = null!;

        public string VeterinarPrezime { get; set; } = null!;

        public bool UslugaPozitivna { get; set; }

        public bool VeterinarPozitivan { get; set; }

        public string? Komentar { get; set; }

        public DateTime DatumDodavanja { get; set; }

        public List<RecenzijaOdgovori> Odgovori { get; set; } = new List<RecenzijaOdgovori>();
    }
}
