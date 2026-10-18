using eVet.Model.Constants;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    // Čitanje je dozvoljeno svim prijavljenim korisnicima, a izmjene samo vlasniku stanice.
    public abstract class VlasnikCRUDController<TModel, TSearch, TInsert, TUpdate> : BaseCRUDController<TModel, TSearch, TInsert, TUpdate> where TSearch : BaseSearchObject where TModel : class
    {
        protected VlasnikCRUDController(ICRUDService<TModel, TSearch, TInsert, TUpdate> service) : base(service)
        {
        }

        [Authorize(Roles = Uloge.Vlasnik)]
        public override Task<TModel> Insert(TInsert request, CancellationToken cancellationToken)
        {
            return base.Insert(request, cancellationToken);
        }

        [Authorize(Roles = Uloge.Vlasnik)]
        public override Task<TModel> Update(int id, TUpdate request, CancellationToken cancellationToken)
        {
            return base.Update(id, request, cancellationToken);
        }

        [Authorize(Roles = Uloge.Vlasnik)]
        public override Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            return base.Delete(id, cancellationToken);
        }
    }
}
