using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;

namespace eVet.Messaging
{
    /// <summary>
    /// Singleton: jedna konekcija i jedan kanal za cijeli API, umjesto nove konekcije pri svakoj poruci.
    /// </summary>
    public sealed class RabbitMqPublisher : IPorukePublisher, IAsyncDisposable
    {
        private readonly RabbitMqSettings _settings;
        private readonly ILogger<RabbitMqPublisher> _logger;
        private readonly SemaphoreSlim _zakljucavanje = new(1, 1);
        private IConnection? _konekcija;
        private IChannel? _kanal;

        public RabbitMqPublisher(IOptions<RabbitMqSettings> settings, ILogger<RabbitMqPublisher> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task PosaljiEmailAsync(EmailPoruka poruka, CancellationToken cancellationToken = default)
        {
            var tijelo = JsonSerializer.SerializeToUtf8Bytes(poruka);

            // Kanal nije thread-safe, pa se objave serijalizuju.
            await _zakljucavanje.WaitAsync(cancellationToken);
            try
            {
                var kanal = await GetKanalAsync(cancellationToken);
                await kanal.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: RedoviPoruka.Emailovi,
                    mandatory: false,
                    basicProperties: new BasicProperties { Persistent = true, ContentType = "application/json" },
                    body: tijelo,
                    cancellationToken: cancellationToken);
            }
            finally
            {
                _zakljucavanje.Release();
            }

            _logger.LogInformation("E-mail \"{Naslov}\" za {Primalac} poslan na red {Red}", poruka.Naslov, poruka.Primalac, RedoviPoruka.Emailovi);
        }

        private async Task<IChannel> GetKanalAsync(CancellationToken cancellationToken)
        {
            if (_kanal is { IsOpen: true })
            {
                return _kanal;
            }

            _konekcija ??= await _settings.KreirajFactory().CreateConnectionAsync("eVet.API", cancellationToken);
            _kanal = await _konekcija.CreateChannelAsync(cancellationToken: cancellationToken);
            await RedoviPoruka.DeklarisiAsync(_kanal, cancellationToken);
            return _kanal;
        }

        public async ValueTask DisposeAsync()
        {
            if (_kanal != null) await _kanal.DisposeAsync();
            if (_konekcija != null) await _konekcija.DisposeAsync();
            _zakljucavanje.Dispose();
        }
    }
}
