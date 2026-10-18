using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IVrsteUslugaService : ICRUDService<VrstaUsluge, NazivSearchObject, VrstaUslugeUpsertRequest, VrstaUslugeUpsertRequest>
    {
    }
}
