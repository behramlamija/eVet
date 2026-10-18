using eVet.Model;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IObavijestiService : IService<Obavijesti, ObavijestiSearchObject>
    {
        Task<int> GetBrojNeprocitanihAsync(CancellationToken cancellationToken = default);

        Task OznaciProcitanoAsync(int id, CancellationToken cancellationToken = default);

        Task OznaciSveProcitanoAsync(CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
