using Microsoft.AspNetCore.Authorization;
using eVet.Model;
using eVet.Model.Constants;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;

namespace eVet.API.Controllers
{
    [Authorize(Roles = Uloge.Veterinar)]
    public class ObavezeController : BaseCRUDController<Obaveze, ObavezeSearchObject, ObavezeUpsertRequest, ObavezeUpsertRequest>
    {
        public ObavezeController(IObavezeService service) : base(service)
        {
        }
    }
}
