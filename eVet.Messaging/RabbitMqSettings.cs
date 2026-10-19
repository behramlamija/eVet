using RabbitMQ.Client;
using System.ComponentModel.DataAnnotations;

namespace eVet.Messaging
{
    // Vrijednosti se čitaju iz .env datoteke (RabbitMq__*).
    public class RabbitMqSettings
    {
        public const string SectionName = "RabbitMq";

        [Required]
        public string Host { get; set; } = null!;

        [Range(1, 65535)]
        public int Port { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public ConnectionFactory KreirajFactory() => new()
        {
            HostName = Host,
            Port = Port,
            UserName = Username,
            Password = Password,
            AutomaticRecoveryEnabled = true
        };
    }
}
