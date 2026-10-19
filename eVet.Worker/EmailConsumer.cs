using eVet.Messaging;
using eVet.Worker.Email;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text.Json;

namespace eVet.Worker
{
    /// <summary>
    /// Sluša red e-mail poruka i šalje e-mailove. Neuspjelo slanje se ponavlja s eksponencijalnim čekanjem,
    /// a poruka koja ni tada ne uspije prebacuje se u red neuspjelih (dead-letter).
    /// </summary>
    public class EmailConsumer : BackgroundService
    {
        private static readonly TimeSpan[] CekanjaIzmeduPokusaja =
        {
            TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(8)
        };

        private readonly RabbitMqSettings _rabbitMq;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<EmailConsumer> _logger;
        private IConnection? _konekcija;
        private IChannel? _kanal;

        public EmailConsumer(IOptions<RabbitMqSettings> rabbitMq, IEmailSender emailSender, ILogger<EmailConsumer> logger)
        {
            _rabbitMq = rabbitMq.Value;
            _emailSender = emailSender;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _konekcija = await PoveziSeAsync(stoppingToken);
            _kanal = await _konekcija.CreateChannelAsync(cancellationToken: stoppingToken);
            await RedoviPoruka.DeklarisiAsync(_kanal, stoppingToken);
            await _kanal.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_kanal);
            consumer.ReceivedAsync += (_, ea) => ObradiAsync(ea, stoppingToken);

            await _kanal.BasicConsumeAsync(RedoviPoruka.Emailovi, autoAck: false, consumer, stoppingToken);
            _logger.LogInformation("Worker sluša red {Red}", RedoviPoruka.Emailovi);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task ObradiAsync(BasicDeliverEventArgs ea, CancellationToken stoppingToken)
        {
            EmailPoruka? poruka;
            try
            {
                poruka = JsonSerializer.Deserialize<EmailPoruka>(ea.Body.Span);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Poruka {DeliveryTag} nije ispravan JSON i prebacuje se u red neuspjelih", ea.DeliveryTag);
                await _kanal!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, stoppingToken);
                return;
            }

            if (poruka == null)
            {
                _logger.LogError("Poruka {DeliveryTag} je prazna i prebacuje se u red neuspjelih", ea.DeliveryTag);
                await _kanal!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, stoppingToken);
                return;
            }

            for (int pokusaj = 0; ; pokusaj++)
            {
                try
                {
                    await _emailSender.PosaljiAsync(poruka, stoppingToken);
                    await _kanal!.BasicAckAsync(ea.DeliveryTag, multiple: false, stoppingToken);
                    _logger.LogInformation("E-mail \"{Naslov}\" poslan na {Primalac}", poruka.Naslov, poruka.Primalac);
                    return;
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    if (pokusaj >= CekanjaIzmeduPokusaja.Length)
                    {
                        _logger.LogError(ex, "E-mail \"{Naslov}\" za {Primalac} nije poslan ni nakon {Broj} pokušaja; poruka ide u red {Red}",
                            poruka.Naslov, poruka.Primalac, pokusaj + 1, RedoviPoruka.EmailoviNeuspjeli);
                        await _kanal!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, stoppingToken);
                        return;
                    }

                    var cekanje = CekanjaIzmeduPokusaja[pokusaj];
                    _logger.LogWarning(ex, "Slanje e-maila \"{Naslov}\" za {Primalac} nije uspjelo (pokušaj {Pokusaj}); novi pokušaj za {Sekunde} s",
                        poruka.Naslov, poruka.Primalac, pokusaj + 1, cekanje.TotalSeconds);
                    await Task.Delay(cekanje, stoppingToken);
                }
            }
        }

        // RabbitMQ se u Dockeru može pokretati sporije od Workera, pa se konekcija pokušava više puta.
        private async Task<IConnection> PoveziSeAsync(CancellationToken stoppingToken)
        {
            var cekanje = TimeSpan.FromSeconds(2);
            while (true)
            {
                try
                {
                    return await _rabbitMq.KreirajFactory().CreateConnectionAsync("eVet.Worker", stoppingToken);
                }
                catch (BrokerUnreachableException ex)
                {
                    _logger.LogWarning(ex, "RabbitMQ ({Host}:{Port}) nije dostupan; novi pokušaj za {Sekunde} s", _rabbitMq.Host, _rabbitMq.Port, cekanje.TotalSeconds);
                    await Task.Delay(cekanje, stoppingToken);
                    cekanje = TimeSpan.FromSeconds(Math.Min(cekanje.TotalSeconds * 2, 30));
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            if (_kanal != null) await _kanal.DisposeAsync();
            if (_konekcija != null) await _konekcija.DisposeAsync();
        }
    }
}
