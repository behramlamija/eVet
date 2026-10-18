namespace eVet.Model.SearchObjects
{
    public class BaseSearchObject
    {
        public const int DefaultPageSize = 20;

        public const int MaxPageSize = 100;

        public int? Page { get; set; }

        public int? PageSize { get; set; }
    }
}
