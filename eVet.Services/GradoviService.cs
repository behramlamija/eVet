using eVet.Model.Exceptions;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    public class GradoviService : BaseCRUDService<Model.Grad, NazivSearchObject, Grad, GradUpsertRequest, GradUpsertRequest>, IGradoviService
    {
        public GradoviService(EVetDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override string NazivEntiteta => "Grad";

        protected override IQueryable<Grad> AddFilter(NazivSearchObject search, IQueryable<Grad> query)
        {
            if (!string.IsNullOrWhiteSpace(search.Naziv))
            {
                query = query.Where(x => x.Naziv.StartsWith(search.Naziv));
            }

            return query;
        }

        protected override Task BeforeInsertAsync(GradUpsertRequest request, Grad entity, CancellationToken cancellationToken)
        {
            return ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv, $"Grad '{request.Naziv}' već postoji.", cancellationToken);
        }

        protected override Task BeforeUpdateAsync(GradUpsertRequest request, Grad entity, CancellationToken cancellationToken)
        {
            return ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv && x.GradId != entity.GradId, $"Grad '{request.Naziv}' već postoji.", cancellationToken);
        }

        protected override async Task BeforeDeleteAsync(Grad entity, CancellationToken cancellationToken)
        {
            if (await Context.Korisnici.AnyAsync(x => x.GradRodjenjaId == entity.GradId, cancellationToken))
            {
                throw new BusinessException("Grad nije moguće obrisati jer je naveden kao mjesto rođenja uposlenika.");
            }

            if (await Context.Termini.AnyAsync(x => x.GradPrevozaId == entity.GradId, cancellationToken))
            {
                throw new BusinessException("Grad nije moguće obrisati jer je naveden kao adresa prevoza u postojećim terminima.");
            }
        }
    }
}
