using eVet.Model.Enums;

namespace eVet.Model.SearchObjects
{
    public class LjubimciSearchObject : BaseSearchObject
    {
        public string? FTS { get; set; }

        public int? VrstaLjubimcaId { get; set; }

        public int? RasaId { get; set; }

        public Spol? Spol { get; set; }

        public int? VlasnikId { get; set; }

        public int? StarostOd { get; set; }

        public int? StarostDo { get; set; }

        // Vakcinisan = ima završen termin s uslugom vakcinacije u posljednjih godinu dana.
        public bool? JeVakcinisan { get; set; }
    }
}
