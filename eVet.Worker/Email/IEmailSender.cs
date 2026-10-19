using eVet.Messaging;

namespace eVet.Worker.Email
{
    public interface IEmailSender
    {
        Task PosaljiAsync(EmailPoruka poruka, CancellationToken cancellationToken);
    }
}
