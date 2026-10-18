using eVet.Model;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IService<TModel, TSearch> where TSearch : BaseSearchObject
    {
        Task<PagedResult<TModel>> GetPagedAsync(TSearch search, CancellationToken cancellationToken = default);

        Task<TModel> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
