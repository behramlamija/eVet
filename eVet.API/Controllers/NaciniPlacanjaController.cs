using eVet.Model;
using eVet.Model.SearchObjects;
using eVet.Services;

namespace eVet.API.Controllers
{
    public class NaciniPlacanjaController : BaseController<NacinPlacanja, NazivSearchObject>
    {
        public NaciniPlacanjaController(INaciniPlacanjaService service) : base(service)
        {
        }
    }
}
