using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;

namespace eVet.API.Controllers
{
    public class GradoviController : VlasnikCRUDController<Grad, NazivSearchObject, GradUpsertRequest, GradUpsertRequest>
    {
        public GradoviController(IGradoviService service) : base(service)
        {
        }
    }
}
