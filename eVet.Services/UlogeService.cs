using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;

namespace eVet.Services
{
    // Nazivi uloga se koriste u [Authorize(Roles = ...)], pa se mijenja samo opis;
    // dodavanje i brisanje uloga nisu dozvoljeni.
    public class UlogeService : BaseCRUDService<Model.Uloga, NazivSearchObject, Uloga, UlogaUpdateRequest, UlogaUpdateRequest>, IUlogeService
    {
        public UlogeService(EVetDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override string NazivEntiteta => "Uloga";

        protected override IQueryable<Uloga> AddFilter(NazivSearchObject search, IQueryable<Uloga> query)
        {
            if (!string.IsNullOrWhiteSpace(search.Naziv))
            {
                query = query.Where(x => x.Naziv.StartsWith(search.Naziv));
            }

            return query;
        }
    }
}
