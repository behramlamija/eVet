namespace eVet.Model
{
    public class Rasa
    {
        public int RasaId { get; set; }

        public string Naziv { get; set; } = null!;

        public int VrstaLjubimcaId { get; set; }

        public string VrstaLjubimcaNaziv { get; set; } = null!;
    }
}
