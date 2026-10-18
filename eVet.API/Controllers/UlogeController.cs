using eVet.Model;
using eVet.Model.Constants;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;

namespace eVet.API.Controllers
{
    [Authorize(Roles = Uloge.Vlasnik)]
    public class UlogeController : BaseController<Uloga, NazivSearchObject>
    {
        public UlogeController(IUlogeService service) : base(service)
        {
        }
    }
}
