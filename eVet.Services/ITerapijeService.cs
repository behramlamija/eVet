using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface ITerapijeService : ICRUDService<Terapije, TerapijeSearchObject, TerapijeUpsertRequest, TerapijeUpsertRequest>
    {
    }
}
