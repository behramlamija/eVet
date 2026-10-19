using RabbitMQ.Client;

namespace eVet.Messaging
{
    /// <summary>
    /// Nazivi redova i njihova deklaracija; isti kod koriste i API (publisher) i Worker (consumer).
    /// </summary>
    public static class RedoviPoruka
    {
        public const string Emailovi = "evet.emailovi";

        /// <summary>Poruke koje Worker nije uspio obraditi ni nakon ponovnih pokušaja.</summary>
        public const string EmailoviNeuspjeli = "evet.emailovi.neuspjeli";

        public static async Task DeklarisiAsync(IChannel kanal, CancellationToken cancellationToken = default)
        {
            await kanal.QueueDeclareAsync(EmailoviNeuspjeli, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);

            await kanal.QueueDeclareAsync(Emailovi, durable: true, exclusive: false, autoDelete: false,
                arguments: new Dictionary<string, object?>
                {
                    ["x-dead-letter-exchange"] = string.Empty,
                    ["x-dead-letter-routing-key"] = EmailoviNeuspjeli
                },
                cancellationToken: cancellationToken);
        }
    }
}
