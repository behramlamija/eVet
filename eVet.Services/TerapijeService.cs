using eVet.Model.Constants;
using eVet.Model.Enums;
using eVet.Model.Exceptions;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    public class TerapijeService : BaseCRUDService<Model.Terapije, TerapijeSearchObject, Terapija, TerapijeUpsertRequest, TerapijeUpsertRequest>, ITerapijeService
    {
        private readonly ICurrentUserService _currentUser;

        public TerapijeService(EVetDbContext context, IMapper mapper, ICurrentUserService currentUser) : base(context, mapper)
        {
            _currentUser = currentUser;
        }

        protected override string NazivEntiteta => "Terapija";

        // Klijent vidi samo terapije svojih ljubimaca.
        protected override IQueryable<Terapija> ApplyPristup(IQueryable<Terapija> query)
        {
            if (_currentUser.IsInRole(Uloge.Vlasnik) || _currentUser.IsInRole(Uloge.Veterinar))
            {
                return query;
            }

            int korisnikId = _currentUser.KorisnikId;
            return query.Where(x => x.Ljubimac.KorisnikId == korisnikId);
        }

        protected override IQueryable<Terapija> AddFilter(TerapijeSearchObject search, IQueryable<Terapija> query)
        {
            if (search.LjubimacId.HasValue)
            {
                query = query.Where(x => x.LjubimacId == search.LjubimacId);
            }

            if (!string.IsNullOrWhiteSpace(search.FTS))
            {
                query = query.Where(x => x.Dijagnoza.Contains(search.FTS)
                    || (x.Lijek != null && x.Lijek.Contains(search.FTS))
                    || x.Ljubimac.Ime.Contains(search.FTS));
            }

            if (search.DatumOd.HasValue)
            {
                query = query.Where(x => x.DatumTerapije >= search.DatumOd);
            }

            if (search.DatumDo.HasValue)
            {
                var doKraja = search.DatumDo.Value.Date.AddDays(1);
                query = query.Where(x => x.DatumTerapije < doKraja);
            }

            return query;
        }

        protected override IQueryable<Terapija> AddInclude(IQueryable<Terapija> query)
        {
            return query.Include(x => x.Ljubimac).Include(x => x.Veterinar).Include(x => x.Termin);
        }

        protected override async Task BeforeInsertAsync(TerapijeUpsertRequest request, Terapija entity, CancellationToken cancellationToken)
        {
            int veterinarId = _currentUser.KorisnikId;
            entity.Veterinar = await Context.Korisnici.FirstAsync(x => x.KorisnikId == veterinarId, cancellationToken);
            entity.DatumTerapije = DateTime.UtcNow;
            await PostaviLjubimcaITerminAsync(request, entity, cancellationToken);
        }

        protected override async Task BeforeUpdateAsync(TerapijeUpsertRequest request, Terapija entity, CancellationToken cancellationToken)
        {
            ProvjeriAutora(entity);
            await PostaviLjubimcaITerminAsync(request, entity, cancellationToken);
        }

        protected override Task BeforeDeleteAsync(Terapija entity, CancellationToken cancellationToken)
        {
            ProvjeriAutora(entity);
            return Task.CompletedTask;
        }

        private void ProvjeriAutora(Terapija entity)
        {
            if (entity.VeterinarId != _currentUser.KorisnikId)
            {
                throw new ForbiddenException("Terapiju može mijenjati ili brisati samo veterinar koji ju je propisao.");
            }
        }

        private async Task PostaviLjubimcaITerminAsync(TerapijeUpsertRequest request, Terapija entity, CancellationToken cancellationToken)
        {
            entity.Ljubimac = await Context.Ljubimci.FirstOrDefaultAsync(x => x.LjubimacId == request.LjubimacId, cancellationToken)
                ?? throw new BusinessException("Odabrani ljubimac ne postoji.");

            if (!request.TerminId.HasValue)
            {
                entity.Termin = null;
                return;
            }

            var termin = await Context.Termini.FirstOrDefaultAsync(x => x.TerminId == request.TerminId, cancellationToken)
                ?? throw new BusinessException("Odabrani termin ne postoji.");

            if (termin.LjubimacId != request.LjubimacId)
            {
                throw new BusinessException("Odabrani termin ne pripada odabranom ljubimcu.");
            }

            if (termin.Status != StatusTermina.Potvrdjen && termin.Status != StatusTermina.Zavrsen)
            {
                throw new BusinessException("Terapiju je moguće vezati samo za potvrđen ili završen termin.");
            }

            entity.Termin = termin;
        }
    }
}
