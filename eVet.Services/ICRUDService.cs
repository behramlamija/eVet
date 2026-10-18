using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface ICRUDService<TModel, TSearch, TInsert, TUpdate> : IService<TModel, TSearch> where TModel : class where TSearch : BaseSearchObject
    {
        Task<TModel> InsertAsync(TInsert request, CancellationToken cancellationToken = default);

        Task<TModel> UpdateAsync(int id, TUpdate request, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
