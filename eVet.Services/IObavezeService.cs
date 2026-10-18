using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IObavezeService : ICRUDService<Obaveze, ObavezeSearchObject, ObavezeUpsertRequest, ObavezeUpsertRequest>
    {
    }
}
