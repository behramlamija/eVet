using eVet.Model;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController<TModel, TSearch>:ControllerBase where TSearch:BaseSearchObject 
    {
        protected IService<TModel,TSearch> _service;

        public BaseController(IService<TModel, TSearch> service)
        {
            _service = service;
        }

        [HttpGet]
        public PagedResult<TModel> GetAll([FromQuery] TSearch searchObject)
        {
            return _service.GetPaged(searchObject);
        }

        [HttpGet("{id}")]
        public TModel GetById(int id)
        {
            return _service.GetById(id);
        }

    }
}
