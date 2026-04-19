using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace eVet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KorisniciController : ControllerBase
    {

        protected IKorisniciService _service;

        public KorisniciController(IKorisniciService service)
        {
            _service = service;
        }

        [HttpGet]
        public PagedResult<Korisnici> GetAll([FromQuery]KorisniciSearchObject searchObject)
        {
            return _service.GetAll(searchObject);
        }


        [HttpPost]
        public Korisnici Insert(KorisniciInsertRequest request)
        {
            return _service.Insert(request);
        }


        [HttpPut("{id}")]
        public Korisnici Update(int id, KorisniciUpdateRequest request)
        {
            return _service.Update(id, request);

        }
    }
}
