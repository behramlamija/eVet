using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IPromocijeService : ICRUDService<Promocije, PromocijeSearchObject, PromocijeUpsertRequest, PromocijeUpsertRequest>
    {
        Task<AktiviranePromocije> AktivirajAsync(int promocijaId, CancellationToken cancellationToken = default);

        Task<PagedResult<AktiviranePromocije>> GetMojeAktiviraneAsync(BaseSearchObject search, CancellationToken cancellationToken = default);
    }
}
