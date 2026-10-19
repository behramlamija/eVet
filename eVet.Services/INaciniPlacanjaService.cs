using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface INaciniPlacanjaService : IService<NacinPlacanja, NazivSearchObject>
    {
        Task<NacinPlacanja> UpdateAsync(int id, NazivUpsertRequest request, CancellationToken cancellationToken = default);
    }
}
