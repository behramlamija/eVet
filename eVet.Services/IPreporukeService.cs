using eVet.Model;

namespace eVet.Services
{
    public interface IPreporukeService
    {
        Task<List<PreporukeUsluga>> GetPreporukeAsync(IReadOnlyCollection<int> uslugeIds, CancellationToken cancellationToken = default);
    }
}
