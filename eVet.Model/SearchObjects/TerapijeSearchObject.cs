namespace eVet.Model.SearchObjects
{
    public class TerapijeSearchObject : BaseSearchObject
    {
        public int? LjubimacId { get; set; }

        public string? FTS { get; set; }

        public DateTime? DatumOd { get; set; }

        public DateTime? DatumDo { get; set; }
    }
}
