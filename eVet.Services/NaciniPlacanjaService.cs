using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;

namespace eVet.Services
{
    // Logika plaćanja koristi ID načina plaćanja (NaciniPlacanja konstante), pa se naziv smije mijenjati,
    // a dodavanje i brisanje nisu dozvoljeni jer novi način plaćanja ne bi imao implementaciju.
    public class NaciniPlacanjaService : BaseCRUDService<Model.NacinPlacanja, NazivSearchObject, NacinPlacanja, NazivUpsertRequest, NazivUpsertRequest>, INaciniPlacanjaService
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

        protected override Task BeforeUpdateAsync(NazivUpsertRequest request, NacinPlacanja entity, CancellationToken cancellationToken)
        {
            return ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv && x.NacinPlacanjaId != entity.NacinPlacanjaId,
                $"Način plaćanja '{request.Naziv}' već postoji.", cancellationToken);
        }
    }
}
