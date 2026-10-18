using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;

namespace eVet.API.Controllers
{
    public class SpecijalnostiController : VlasnikCRUDController<Specijalnost, NazivSearchObject, NazivUpsertRequest, NazivUpsertRequest>
    {
        public SpecijalnostiController(ISpecijalnostiService service) : base(service)
        {
        }
    }
}
