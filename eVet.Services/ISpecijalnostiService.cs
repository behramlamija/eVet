using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface ISpecijalnostiService : ICRUDService<Specijalnost, NazivSearchObject, NazivUpsertRequest, NazivUpsertRequest>
    {
    }
}
