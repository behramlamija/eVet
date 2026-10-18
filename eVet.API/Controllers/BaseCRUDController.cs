using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    public class BaseCRUDController<TModel, TSearch, TInsert, TUpdate> : BaseController<TModel, TSearch> where TSearch : BaseSearchObject where TModel : class
    {
        protected readonly ICRUDService<TModel, TSearch, TInsert, TUpdate> _crudService;

        public BaseCRUDController(ICRUDService<TModel, TSearch, TInsert, TUpdate> service) : base(service)
        {
            _crudService = service;
        }

        [HttpPost]
        public virtual Task<TModel> Insert(TInsert request, CancellationToken cancellationToken)
        {
            return _crudService.InsertAsync(request, cancellationToken);
        }

        [HttpPut("{id}")]
        public virtual Task<TModel> Update(int id, TUpdate request, CancellationToken cancellationToken)
        {
            return _crudService.UpdateAsync(id, request, cancellationToken);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _crudService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
