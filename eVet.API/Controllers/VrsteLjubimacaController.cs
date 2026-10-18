using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;

namespace eVet.API.Controllers
{
    public class VrsteLjubimacaController : VlasnikCRUDController<VrstaLjubimca, NazivSearchObject, NazivUpsertRequest, NazivUpsertRequest>
    {
        public VrsteLjubimacaController(IVrsteLjubimacaService service) : base(service)
        {
        }
    }
}
