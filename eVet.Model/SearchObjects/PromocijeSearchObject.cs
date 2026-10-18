namespace eVet.Model.SearchObjects
{
    public class PromocijeSearchObject : BaseSearchObject
    {
        public string? FTS { get; set; }

        public int? UslugaId { get; set; }

        public bool? SamoVazece { get; set; }
    }
}
