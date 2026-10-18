using eVet.Model;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BaseController<TModel, TSearch> : ControllerBase where TSearch : BaseSearchObject
    {
        protected readonly IService<TModel, TSearch> _service;

        public BaseController(IService<TModel, TSearch> service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual Task<PagedResult<TModel>> GetAll([FromQuery] TSearch searchObject, CancellationToken cancellationToken)
        {
            return _service.GetPagedAsync(searchObject, cancellationToken);
        }

        [HttpGet("{id}")]
        public virtual Task<TModel> GetById(int id, CancellationToken cancellationToken)
        {
            return _service.GetByIdAsync(id, cancellationToken);
        }
    }
}
