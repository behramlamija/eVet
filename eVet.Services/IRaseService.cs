using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IRaseService : ICRUDService<Rasa, RasaSearchObject, RasaUpsertRequest, RasaUpsertRequest>
    {
    }
}
