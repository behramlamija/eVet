using eVet.Model;
using eVet.Model.SearchObjects;
using eVet.Services;
using eVet.Services.Database;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VrstaUslugeController:BaseController<Model.VrstaUsluge,VrstaUslugeSearchObject>
    {

        public VrstaUslugeController(IVrsteUslugaService service):base(service)
        {
            _service = service;
        }

    }
}
