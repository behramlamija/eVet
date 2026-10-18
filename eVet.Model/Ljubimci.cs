using eVet.Model.Enums;

namespace eVet.Model
{
    public class Ljubimci
    {
        public int LjubimacId { get; set; }

        public int KorisnikId { get; set; }

        public string KorisnikIme { get; set; } = null!;

        public string KorisnikPrezime { get; set; } = null!;

        public string Ime { get; set; } = null!;

        public int RasaId { get; set; }

        public string RasaNaziv { get; set; } = null!;

        public int VrstaLjubimcaId { get; set; }

        public string VrstaLjubimcaNaziv { get; set; } = null!;

        public DateOnly? DatumRodjenja { get; set; }

        public Spol Spol { get; set; }

        public decimal? Tezina { get; set; }

        public string? Napomena { get; set; }

        public bool ImaSliku { get; set; }
    }
}
