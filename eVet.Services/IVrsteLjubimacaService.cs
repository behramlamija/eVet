using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IVrsteLjubimacaService : ICRUDService<VrstaLjubimca, NazivSearchObject, NazivUpsertRequest, NazivUpsertRequest>
    {
    }
}
