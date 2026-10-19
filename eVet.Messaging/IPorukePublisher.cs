namespace eVet.Messaging
{
    public interface IPorukePublisher
    {
        Task PosaljiEmailAsync(EmailPoruka poruka, CancellationToken cancellationToken = default);
    }
}
