using eVet.Model.Exceptions;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    public class SpecijalnostiService : BaseCRUDService<Model.Specijalnost, NazivSearchObject, Specijalnost, NazivUpsertRequest, NazivUpsertRequest>, ISpecijalnostiService
    {
        public SpecijalnostiService(EVetDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override string NazivEntiteta => "Specijalnost";

        protected override IQueryable<Specijalnost> AddFilter(NazivSearchObject search, IQueryable<Specijalnost> query)
        {
            if (!string.IsNullOrWhiteSpace(search.Naziv))
            {
                query = query.Where(x => x.Naziv.StartsWith(search.Naziv));
            }

            return query;
        }

        protected override Task BeforeInsertAsync(NazivUpsertRequest request, Specijalnost entity, CancellationToken cancellationToken)
        {
            return ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv, $"Specijalnost '{request.Naziv}' već postoji.", cancellationToken);
        }

        protected override Task BeforeUpdateAsync(NazivUpsertRequest request, Specijalnost entity, CancellationToken cancellationToken)
        {
            return ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv && x.SpecijalnostId != entity.SpecijalnostId, $"Specijalnost '{request.Naziv}' već postoji.", cancellationToken);
        }

        protected override async Task BeforeDeleteAsync(Specijalnost entity, CancellationToken cancellationToken)
        {
            if (await Context.Korisnici.AnyAsync(x => x.SpecijalnostId == entity.SpecijalnostId, cancellationToken))
            {
                throw new BusinessException("Specijalnost nije moguće obrisati jer je dodijeljena veterinarima.");
            }
        }
    }
}
