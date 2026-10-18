using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;

namespace eVet.API.Controllers
{
    public class VrstaUslugeController : VlasnikCRUDController<VrstaUsluge, NazivSearchObject, VrstaUslugeUpsertRequest, VrstaUslugeUpsertRequest>
    {
        public VrstaUslugeController(IVrsteUslugaService service) : base(service)
        {
        }
    }
}
