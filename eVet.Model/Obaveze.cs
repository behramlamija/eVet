namespace eVet.Model
{
    public class Obaveze
    {
        public int ObavezaId { get; set; }

        public string Naslov { get; set; } = null!;

        public string? Opis { get; set; }

        public DateTime DatumVrijeme { get; set; }

        public bool JeZavrsena { get; set; }
    }
}
