namespace eVet.Services
{
    public interface ICurrentUserService
    {
        int KorisnikId { get; }

        bool IsInRole(string uloga);
    }
}
