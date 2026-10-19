using eVet.Messaging;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace eVet.Worker.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _settings;

        public SmtpEmailSender(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task PosaljiAsync(EmailPoruka poruka, CancellationToken cancellationToken)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(_settings.NazivPosiljaoca, _settings.Posiljalac));
            mail.To.Add(new MailboxAddress(poruka.ImePrimaoca, poruka.Primalac));
            mail.Subject = poruka.Naslov;
            mail.Body = new BodyBuilder
            {
                TextBody = $"Poštovani/a {poruka.ImePrimaoca},\n\n{poruka.Tekst}\n\nVaša PetCity veterinarska stanica"
            }.ToMessageBody();

            using var klijent = new SmtpClient();
            await klijent.ConnectAsync(_settings.Host, _settings.Port, _settings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None, cancellationToken);

            if (!string.IsNullOrWhiteSpace(_settings.Username))
            {
                await klijent.AuthenticateAsync(_settings.Username, _settings.Password ?? string.Empty, cancellationToken);
            }

            await klijent.SendAsync(mail, cancellationToken);
            await klijent.DisconnectAsync(true, cancellationToken);
        }
    }
}
