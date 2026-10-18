namespace eVet.Services.Security
{
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        public string Key { get; set; } = null!;

        public string Issuer { get; set; } = null!;

        public string Audience { get; set; } = null!;

        public int TrajanjeMinuta { get; set; }
    }
}
