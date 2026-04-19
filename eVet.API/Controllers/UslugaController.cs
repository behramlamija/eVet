using eVet.Model;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UslugaController :BaseController<Usluge,UslugeSearchObject>
    {

        public UslugaController(IUslugeService service): base(service)
        {
           
        }

        

    }
}
