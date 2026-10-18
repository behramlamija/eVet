using eVet.Model.Exceptions;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    public class VrsteLjubimacaService : BaseCRUDService<Model.VrstaLjubimca, NazivSearchObject, VrstaLjubimca, NazivUpsertRequest, NazivUpsertRequest>, IVrsteLjubimacaService
    {
        public VrsteLjubimacaService(EVetDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override string NazivEntiteta => "Vrsta ljubimca";

        protected override IQueryable<VrstaLjubimca> AddFilter(NazivSearchObject search, IQueryable<VrstaLjubimca> query)
        {
            if (!string.IsNullOrWhiteSpace(search.Naziv))
            {
                query = query.Where(x => x.Naziv.StartsWith(search.Naziv));
            }

            return query;
        }

        protected override Task BeforeInsertAsync(NazivUpsertRequest request, VrstaLjubimca entity, CancellationToken cancellationToken)
        {
            return ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv, $"Vrsta ljubimca '{request.Naziv}' već postoji.", cancellationToken);
        }

        protected override Task BeforeUpdateAsync(NazivUpsertRequest request, VrstaLjubimca entity, CancellationToken cancellationToken)
        {
            return ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv && x.VrstaLjubimcaId != entity.VrstaLjubimcaId, $"Vrsta ljubimca '{request.Naziv}' već postoji.", cancellationToken);
        }

        protected override async Task BeforeDeleteAsync(VrstaLjubimca entity, CancellationToken cancellationToken)
        {
            if (await Context.Rase.AnyAsync(x => x.VrstaLjubimcaId == entity.VrstaLjubimcaId, cancellationToken))
            {
                throw new BusinessException("Vrstu ljubimca nije moguće obrisati jer joj pripadaju rase. Najprije obrišite ili premjestite te rase.");
            }
        }
    }
}
