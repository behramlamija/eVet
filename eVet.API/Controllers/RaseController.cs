using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;

namespace eVet.API.Controllers
{
    public class RaseController : VlasnikCRUDController<Rasa, RasaSearchObject, RasaUpsertRequest, RasaUpsertRequest>
    {
        public RaseController(IRaseService service) : base(service)
        {
        }
    }
}
