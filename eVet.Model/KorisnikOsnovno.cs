namespace eVet.Model
{
    public class KorisnikOsnovno
    {
        public int KorisnikId { get; set; }

        public string Ime { get; set; } = null!;

        public string Prezime { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Telefon { get; set; }

        public string? Zvanje { get; set; }

        public string? SpecijalnostNaziv { get; set; }

        public bool ImaSliku { get; set; }
    }
}
