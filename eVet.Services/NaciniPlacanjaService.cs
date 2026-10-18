using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;

namespace eVet.Services
{
    // Načini plaćanja su vezani za poslovnu logiku (gotovina / PayPal), pa se samo čitaju.
    public class NaciniPlacanjaService : BaseService<Model.NacinPlacanja, NazivSearchObject, NacinPlacanja>, INaciniPlacanjaService
    {
        public NaciniPlacanjaService(EVetDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override string NazivEntiteta => "Način plaćanja";

        protected override IQueryable<NacinPlacanja> AddFilter(NazivSearchObject search, IQueryable<NacinPlacanja> query)
        {
            if (!string.IsNullOrWhiteSpace(search.Naziv))
            {
                query = query.Where(x => x.Naziv.StartsWith(search.Naziv));
            }

            return query;
        }
    }
}
