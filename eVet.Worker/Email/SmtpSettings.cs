using System.ComponentModel.DataAnnotations;

namespace eVet.Worker.Email
{
    // Vrijednosti se čitaju iz .env datoteke (Smtp__*).
    public class SmtpSettings
    {
        public const string SectionName = "Smtp";

        [Required]
        public string Host { get; set; } = null!;

        [Range(1, 65535)]
        public int Port { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public bool UseSsl { get; set; }

        [Required, EmailAddress]
        public string Posiljalac { get; set; } = null!;

        [Required]
        public string NazivPosiljaoca { get; set; } = null!;
    }
}
