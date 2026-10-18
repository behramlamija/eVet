using eVet.Model.Exceptions;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using eVet.Services.Termini;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    public class UslugeService : BaseCRUDService<Model.Usluge, UslugeSearchObject, Usluga, UslugeUpsertRequest, UslugeUpsertRequest>, IUslugeService
    {
        public UslugeService(EVetDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override string NazivEntiteta => "Usluga";

        protected override IQueryable<Usluga> AddFilter(UslugeSearchObject search, IQueryable<Usluga> query)
        {
            if (!string.IsNullOrWhiteSpace(search.FTS))
            {
                query = query.Where(x => x.Naziv.Contains(search.FTS) || (x.Opis != null && x.Opis.Contains(search.FTS)));
            }

            if (search.VrstaId.HasValue)
            {
                query = query.Where(x => x.VrstaId == search.VrstaId);
            }

            if (search.Trajanje.HasValue)
            {
                query = query.Where(x => x.Trajanje <= search.Trajanje);
            }

            return query;
        }

        protected override IQueryable<Usluga> AddInclude(IQueryable<Usluga> query)
        {
            return query.Include(x => x.Vrsta);
        }

        protected override async Task BeforeInsertAsync(UslugeUpsertRequest request, Usluga entity, CancellationToken cancellationToken)
        {
            await ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv, $"Usluga '{request.Naziv}' već postoji.", cancellationToken);
            entity.Vrsta = await UcitajVrstuAsync(request.VrstaId, cancellationToken);
            entity.DatumObjavljivanja = DateTime.UtcNow;
        }

        protected override async Task BeforeUpdateAsync(UslugeUpsertRequest request, Usluga entity, CancellationToken cancellationToken)
        {
            await ProvjeriDaNePostojiAsync(x => x.Naziv == request.Naziv && x.UslugaId != entity.UslugaId, $"Usluga '{request.Naziv}' već postoji.", cancellationToken);
            entity.Vrsta = await UcitajVrstuAsync(request.VrstaId, cancellationToken);
        }

        protected override async Task BeforeDeleteAsync(Usluga entity, CancellationToken cancellationToken)
        {
            if (await Context.StavkeTermina.AnyAsync(x => x.UslugaId == entity.UslugaId && TerminStateMachine.AktivniStatusi.Contains(x.Termin.Status), cancellationToken))
            {
                throw new BusinessException("Uslugu nije moguće obrisati jer je dio zakazanih termina koji još nisu završeni ili otkazani.");
            }

            if (await Context.Promocije.AnyAsync(x => x.UslugaId == entity.UslugaId, cancellationToken))
            {
                throw new BusinessException("Uslugu nije moguće obrisati jer za nju postoje promocije. Najprije obrišite te promocije.");
            }
        }

        private async Task<VrstaUsluge> UcitajVrstuAsync(int vrstaId, CancellationToken cancellationToken)
        {
            return await Context.VrsteUsluga.FirstOrDefaultAsync(x => x.VrstaId == vrstaId, cancellationToken)
                ?? throw new BusinessException("Odabrana vrsta usluge ne postoji.");
        }
    }
}
