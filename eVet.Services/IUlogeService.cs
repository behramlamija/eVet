using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IUlogeService : IService<Uloga, NazivSearchObject>
    {
        Task<Uloga> UpdateAsync(int id, UlogaUpdateRequest request, CancellationToken cancellationToken = default);
    }
}
