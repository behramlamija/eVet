using eVet.Model.Enums;

namespace eVet.Model.SearchObjects
{
    public class TerminiSearchObject : BaseSearchObject
    {
        public string? FTS { get; set; }

        public StatusTermina? Status { get; set; }

        public DateOnly? DatumOd { get; set; }

        public DateOnly? DatumDo { get; set; }

        public int? VeterinarId { get; set; }

        public int? LjubimacId { get; set; }
    }
}
