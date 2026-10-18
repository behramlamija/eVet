namespace eVet.Model.SearchObjects
{
    public class KorisniciSearchObject : BaseSearchObject
    {
        public string? ImeGTE { get; set; }

        public string? PrezimeGTE { get; set; }

        public string? KorisnickoIme { get; set; }

        public string? Email { get; set; }

        public int? UlogaId { get; set; }

        public bool? JeAktivan { get; set; }

        public int? SpecijalnostId { get; set; }
    }
}
