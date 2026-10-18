namespace eVet.Model
{
    public class RecenzijaOdgovori
    {
        public int RecenzijaOdgovorId { get; set; }

        public string KorisnikIme { get; set; } = null!;

        public string KorisnikPrezime { get; set; } = null!;

        public string Komentar { get; set; } = null!;

        public DateTime DatumDodavanja { get; set; }
    }
}
