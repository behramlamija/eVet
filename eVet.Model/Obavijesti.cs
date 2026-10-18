namespace eVet.Model
{
    public class Obavijesti
    {
        public int ObavijestId { get; set; }

        public string Naslov { get; set; } = null!;

        public string Sadrzaj { get; set; } = null!;

        public bool JePogledana { get; set; }

        public DateTime DatumObavijesti { get; set; }
    }
}
