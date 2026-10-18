using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IGradoviService : ICRUDService<Grad, NazivSearchObject, GradUpsertRequest, GradUpsertRequest>
    {
    }
}
