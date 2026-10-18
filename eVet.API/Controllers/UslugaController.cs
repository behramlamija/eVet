using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;

namespace eVet.API.Controllers
{
    public class UslugaController : VlasnikCRUDController<Usluge, UslugeSearchObject, UslugeUpsertRequest, UslugeUpsertRequest>
    {
        public UslugaController(IUslugeService service) : base(service)
        {
        }
    }
}
