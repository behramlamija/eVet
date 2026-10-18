namespace eVet.Model.Responses
{
    public class ErrorResponse
    {
        public string Poruka { get; set; } = null!;

        public Dictionary<string, string[]>? Greske { get; set; }

        public string? TraceId { get; set; }
    }
}
