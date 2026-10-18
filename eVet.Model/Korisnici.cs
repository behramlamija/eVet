namespace eVet.Model
{
    public class Korisnici
    {
        public int KorisnikId { get; set; }

        public string Ime { get; set; } = null!;

        public string Prezime { get; set; } = null!;

        public string KorisnickoIme { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Telefon { get; set; }

        public bool ImaSliku { get; set; }

        public bool JeAktivan { get; set; }

        public DateTime DatumRegistracije { get; set; }

        public DateOnly? DatumRodjenja { get; set; }

        public int? GradRodjenjaId { get; set; }

        public string? GradRodjenjaNaziv { get; set; }

        public DateOnly? DatumZaposlenja { get; set; }

        public int? GodineIskustva { get; set; }

        public string? Zvanje { get; set; }

        public int? SpecijalnostId { get; set; }

        public string? SpecijalnostNaziv { get; set; }

        public List<Uloga> Uloge { get; set; } = new List<Uloga>();
    }
}
