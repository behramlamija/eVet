using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface ILjubimciService : ICRUDService<Ljubimci, LjubimciSearchObject, LjubimciUpsertRequest, LjubimciUpsertRequest>
    {
    }
}
