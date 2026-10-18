using eVet.Model.Exceptions;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    public class VrsteUslugaService : BaseCRUDService<Model.VrstaUsluge, NazivSearchObject, VrstaUsluge, VrstaUslugeUpsertRequest, VrstaUslugeUpsertRequest>, IVrsteUslugaService
    {
        public VrsteUslugaService(EVetDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override string NazivEntiteta => "Vrsta usluge";

        protected override IQueryable<VrstaUsluge> AddFilter(NazivSearchObject search, IQueryable<VrstaUsluge> query)
        {
            if (!string.IsNullOrWhiteSpace(search.Naziv))
            {
                query = query.Where(x => x.Naziv.StartsWith(search.Naziv));
            }

            return query;
        }

        protected override Task BeforeInsertAsync(VrstaUslugeUpsertRequest request, VrstaUsluge entity, CancellationToken cancellationToken)
        {
            return ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv, $"Vrsta usluge '{request.Naziv}' već postoji.", cancellationToken);
        }

        protected override Task BeforeUpdateAsync(VrstaUslugeUpsertRequest request, VrstaUsluge entity, CancellationToken cancellationToken)
        {
            return ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv && x.VrstaId != entity.VrstaId, $"Vrsta usluge '{request.Naziv}' već postoji.", cancellationToken);
        }

        protected override async Task BeforeDeleteAsync(VrstaUsluge entity, CancellationToken cancellationToken)
        {
            if (await Context.Usluge.AnyAsync(x => x.VrstaId == entity.VrstaId, cancellationToken))
            {
                throw new BusinessException("Vrstu usluge nije moguće obrisati jer joj pripadaju usluge.");
            }
        }
    }
}
