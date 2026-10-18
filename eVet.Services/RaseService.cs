using eVet.Model.Exceptions;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    public class RaseService : BaseCRUDService<Model.Rasa, RasaSearchObject, Rasa, RasaUpsertRequest, RasaUpsertRequest>, IRaseService
    {
        public RaseService(EVetDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override string NazivEntiteta => "Rasa";

        protected override IQueryable<Rasa> AddFilter(RasaSearchObject search, IQueryable<Rasa> query)
        {
            if (!string.IsNullOrWhiteSpace(search.Naziv))
            {
                query = query.Where(x => x.Naziv.StartsWith(search.Naziv));
            }

            if (search.VrstaLjubimcaId.HasValue)
            {
                query = query.Where(x => x.VrstaLjubimcaId == search.VrstaLjubimcaId);
            }

            return query;
        }

        protected override IQueryable<Rasa> AddInclude(IQueryable<Rasa> query)
        {
            return query.Include(x => x.VrstaLjubimca);
        }

        protected override async Task BeforeInsertAsync(RasaUpsertRequest request, Rasa entity, CancellationToken cancellationToken)
        {
            await ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv && x.VrstaLjubimcaId == request.VrstaLjubimcaId,
                $"Rasa '{request.Naziv}' već postoji za odabranu vrstu ljubimca.", cancellationToken);
            entity.VrstaLjubimca = await UcitajVrstuAsync(request.VrstaLjubimcaId, cancellationToken);
        }

        protected override async Task BeforeUpdateAsync(RasaUpsertRequest request, Rasa entity, CancellationToken cancellationToken)
        {
            await ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv && x.VrstaLjubimcaId == request.VrstaLjubimcaId && x.RasaId != entity.RasaId,
                $"Rasa '{request.Naziv}' već postoji za odabranu vrstu ljubimca.", cancellationToken);
            entity.VrstaLjubimca = await UcitajVrstuAsync(request.VrstaLjubimcaId, cancellationToken);
        }

        protected override async Task BeforeDeleteAsync(Rasa entity, CancellationToken cancellationToken)
        {
            if (await Context.Ljubimci.AnyAsync(x => x.RasaId == entity.RasaId, cancellationToken))
            {
                throw new BusinessException("Rasu nije moguće obrisati jer je dodijeljena ljubimcima.");
            }
        }

        private async Task<VrstaLjubimca> UcitajVrstuAsync(int vrstaLjubimcaId, CancellationToken cancellationToken)
        {
            return await Context.VrsteLjubimaca.FirstOrDefaultAsync(x => x.VrstaLjubimcaId == vrstaLjubimcaId, cancellationToken)
                ?? throw new BusinessException("Odabrana vrsta ljubimca ne postoji.");
        }
    }
}
