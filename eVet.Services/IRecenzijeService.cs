using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IRecenzijeService : IService<Recenzije, RecenzijeSearchObject>
    {
        Task<Recenzije> InsertAsync(RecenzijaInsertRequest request, CancellationToken cancellationToken = default);

        Task<Recenzije> OdgovoriAsync(int id, RecenzijaOdgovorRequest request, CancellationToken cancellationToken = default);

        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
