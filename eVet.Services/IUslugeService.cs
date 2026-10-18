using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IUslugeService : ICRUDService<Usluge, UslugeSearchObject, UslugeUpsertRequest, UslugeUpsertRequest>
    {
    }
}
