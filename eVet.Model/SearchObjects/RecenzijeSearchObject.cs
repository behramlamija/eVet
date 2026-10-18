namespace eVet.Model.SearchObjects
{
    public class RecenzijeSearchObject : BaseSearchObject
    {
        public int? VeterinarId { get; set; }

        public int? UslugaId { get; set; }

        public bool? VeterinarPozitivan { get; set; }
    }
}
