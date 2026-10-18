using eVet.Model.Constants;
using eVet.Model.Exceptions;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using eVet.Services.Termini;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    public class PromocijeService : BaseCRUDService<Model.Promocije, PromocijeSearchObject, Promocija, PromocijeUpsertRequest, PromocijeUpsertRequest>, IPromocijeService
    {
        private readonly ICurrentUserService _currentUser;

        public PromocijeService(EVetDbContext context, IMapper mapper, ICurrentUserService currentUser) : base(context, mapper)
        {
            _currentUser = currentUser;
        }

        protected override string NazivEntiteta => "Promocija";

        // Klijent vidi samo promocije koje trenutno važe.
        protected override IQueryable<Promocija> ApplyPristup(IQueryable<Promocija> query)
        {
            if (_currentUser.IsInRole(Uloge.Vlasnik) || _currentUser.IsInRole(Uloge.Veterinar))
            {
                return query;
            }

            return SamoVazece(query);
        }

        protected override IQueryable<Promocija> AddFilter(PromocijeSearchObject search, IQueryable<Promocija> query)
        {
            if (!string.IsNullOrWhiteSpace(search.FTS))
            {
                query = query.Where(x => x.Naziv.Contains(search.FTS) || x.Kod.Contains(search.FTS) || x.Usluga.Naziv.Contains(search.FTS));
            }

            if (search.UslugaId.HasValue)
            {
                query = query.Where(x => x.UslugaId == search.UslugaId);
            }

            if (search.SamoVazece == true)
            {
                query = SamoVazece(query);
            }

            return query;
        }

        protected override IQueryable<Promocija> AddInclude(IQueryable<Promocija> query)
        {
            return query.Include(x => x.Usluga);
        }

        protected override async Task BeforeInsertAsync(PromocijeUpsertRequest request, Promocija entity, CancellationToken cancellationToken)
        {
            await ProvjeriDaNePostojiAsync(x => x.Kod == request.Kod, $"Promocija s kodom '{request.Kod}' već postoji.", cancellationToken);
            entity.Usluga = await UcitajUsluguAsync(request.UslugaId, cancellationToken);
        }

        protected override async Task BeforeUpdateAsync(PromocijeUpsertRequest request, Promocija entity, CancellationToken cancellationToken)
        {
            await ProvjeriDaNePostojiAsync(x => x.Kod == request.Kod && x.PromocijaId != entity.PromocijaId, $"Promocija s kodom '{request.Kod}' već postoji.", cancellationToken);
            entity.Usluga = await UcitajUsluguAsync(request.UslugaId, cancellationToken);
        }

        protected override async Task BeforeDeleteAsync(Promocija entity, CancellationToken cancellationToken)
        {
            if (await Context.Termini.AnyAsync(x => x.AktiviranaPromocija != null
                    && x.AktiviranaPromocija.PromocijaId == entity.PromocijaId
                    && TerminStateMachine.AktivniStatusi.Contains(x.Status), cancellationToken))
            {
                throw new BusinessException("Promociju nije moguće obrisati jer je iskorištena u terminima koji još nisu završeni. Umjesto brisanja je možete deaktivirati.");
            }
        }

        public async Task<Model.AktiviranePromocije> AktivirajAsync(int promocijaId, CancellationToken cancellationToken = default)
        {
            int korisnikId = _currentUser.KorisnikId;

            var promocija = await SamoVazece(Context.Promocije.Include(x => x.Usluga))
                .FirstOrDefaultAsync(x => x.PromocijaId == promocijaId, cancellationToken)
                ?? throw new NotFoundException("Promocija ne postoji ili je istekla.");

            if (await Context.AktiviranePromocije.AnyAsync(x => x.KorisnikId == korisnikId && x.PromocijaId == promocijaId, cancellationToken))
            {
                throw new BusinessException("Ovu promociju ste već aktivirali.");
            }

            var aktivirana = new AktiviranaPromocija
            {
                KorisnikId = korisnikId,
                Promocija = promocija,
                DatumAktiviranja = DateTime.UtcNow
            };

            Context.AktiviranePromocije.Add(aktivirana);
            await Context.SaveChangesAsync(cancellationToken);

            return Mapper.Map<Model.AktiviranePromocije>(aktivirana);
        }

        public Task<Model.PagedResult<Model.AktiviranePromocije>> GetMojeAktiviraneAsync(BaseSearchObject search, CancellationToken cancellationToken = default)
        {
            int korisnikId = _currentUser.KorisnikId;
            var query = Context.AktiviranePromocije.AsNoTracking().Where(x => x.KorisnikId == korisnikId);

            return query
                .OrderBy(x => x.Iskoristena)
                .ThenByDescending(x => x.DatumAktiviranja)
                .ToPagedResultAsync<AktiviranaPromocija, Model.AktiviranePromocije>(query, search, Mapper.Config, cancellationToken);
        }

        private static IQueryable<Promocija> SamoVazece(IQueryable<Promocija> query)
        {
            var sada = DateTime.UtcNow;
            return query.Where(x => x.JeAktivna && x.DatumPocetka <= sada && x.DatumKraja >= sada);
        }

        private async Task<Usluga> UcitajUsluguAsync(int uslugaId, CancellationToken cancellationToken)
        {
            return await Context.Usluge.FirstOrDefaultAsync(x => x.UslugaId == uslugaId, cancellationToken)
                ?? throw new BusinessException("Odabrana usluga ne postoji.");
        }
    }
}
