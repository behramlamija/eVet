using eVet.Model;
using eVet.Model.Constants;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVet.API.Controllers
{
    // Terapije propisuje veterinar; klijent može pregledati terapije svojih ljubimaca.
    public class TerapijeController : BaseCRUDController<Terapije, TerapijeSearchObject, TerapijeUpsertRequest, TerapijeUpsertRequest>
    {
        public TerapijeController(ITerapijeService service) : base(service)
        {
        }

        [Authorize(Roles = Uloge.Veterinar)]
        public override Task<Terapije> Insert(TerapijeUpsertRequest request, CancellationToken cancellationToken)
        {
            return base.Insert(request, cancellationToken);
        }

        [Authorize(Roles = Uloge.Veterinar)]
        public override Task<Terapije> Update(int id, TerapijeUpsertRequest request, CancellationToken cancellationToken)
        {
            return base.Update(id, request, cancellationToken);
        }

        [Authorize(Roles = Uloge.Veterinar)]
        public override Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            return base.Delete(id, cancellationToken);
        }
    }
}
