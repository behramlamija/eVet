namespace eVet.Model
{
    public class VrstaUsluge
    {
        public int VrstaId { get; set; }

        public string Naziv { get; set; } = null!;

        public bool JeOperacija { get; set; }

        public bool JeVakcinacija { get; set; }

        public bool ImaSliku { get; set; }
    }
}
