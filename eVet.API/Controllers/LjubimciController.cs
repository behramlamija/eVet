using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;

namespace eVet.API.Controllers
{
    public class LjubimciController : BaseCRUDController<Ljubimci, LjubimciSearchObject, LjubimciUpsertRequest, LjubimciUpsertRequest>
    {
        public LjubimciController(ILjubimciService service) : base(service)
        {
        }
    }
}
