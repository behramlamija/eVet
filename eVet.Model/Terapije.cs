namespace eVet.Model
{
    public class Terapije
    {
        public int TerapijaId { get; set; }

        public int LjubimacId { get; set; }

        public string LjubimacIme { get; set; } = null!;

        public int VeterinarId { get; set; }

        public string VeterinarIme { get; set; } = null!;

        public string VeterinarPrezime { get; set; } = null!;

        public int? TerminId { get; set; }

        public string? TerminSifra { get; set; }

        public string Dijagnoza { get; set; } = null!;

        public string? Lijek { get; set; }

        public string? Doza { get; set; }

        public string? Napomena { get; set; }

        public DateTime DatumTerapije { get; set; }

        public int? TrajanjeDana { get; set; }

        public bool JeZavrsena { get; set; }

        public DateOnly? DatumKontrolnog { get; set; }
    }
}
