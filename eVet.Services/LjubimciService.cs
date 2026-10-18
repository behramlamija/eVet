using eVet.Model.Constants;
using eVet.Model.Enums;
using eVet.Model.Exceptions;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using eVet.Services.Termini;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    public class LjubimciService : BaseCRUDService<Model.Ljubimci, LjubimciSearchObject, Ljubimac, LjubimciUpsertRequest, LjubimciUpsertRequest>, ILjubimciService
    {
        private readonly ICurrentUserService _currentUser;

        public LjubimciService(EVetDbContext context, IMapper mapper, ICurrentUserService currentUser) : base(context, mapper)
        {
            _currentUser = currentUser;
        }

        protected override string NazivEntiteta => "Ljubimac";

        private bool JeOsoblje => _currentUser.IsInRole(Uloge.Vlasnik) || _currentUser.IsInRole(Uloge.Veterinar);

        protected override IQueryable<Ljubimac> ApplyPristup(IQueryable<Ljubimac> query)
        {
            if (JeOsoblje)
            {
                return query;
            }

            int korisnikId = _currentUser.KorisnikId;
            return query.Where(x => x.KorisnikId == korisnikId);
        }

        protected override IQueryable<Ljubimac> AddFilter(LjubimciSearchObject search, IQueryable<Ljubimac> query)
        {
            if (!string.IsNullOrWhiteSpace(search.FTS))
            {
                query = query.Where(x => x.Ime.Contains(search.FTS)
                    || x.Korisnik.Ime.Contains(search.FTS)
                    || x.Korisnik.Prezime.Contains(search.FTS));
            }

            if (search.VrstaLjubimcaId.HasValue)
            {
                query = query.Where(x => x.Rasa.VrstaLjubimcaId == search.VrstaLjubimcaId);
            }

            if (search.RasaId.HasValue)
            {
                query = query.Where(x => x.RasaId == search.RasaId);
            }

            if (search.Spol.HasValue)
            {
                query = query.Where(x => x.Spol == search.Spol);
            }

            if (search.VlasnikId.HasValue)
            {
                query = query.Where(x => x.KorisnikId == search.VlasnikId);
            }

            var danas = DateOnly.FromDateTime(DateTime.UtcNow);

            if (search.StarostOd.HasValue)
            {
                var rodjenNajkasnije = danas.AddYears(-search.StarostOd.Value);
                query = query.Where(x => x.DatumRodjenja != null && x.DatumRodjenja <= rodjenNajkasnije);
            }

            if (search.StarostDo.HasValue)
            {
                var rodjenNakon = danas.AddYears(-(search.StarostDo.Value + 1));
                query = query.Where(x => x.DatumRodjenja != null && x.DatumRodjenja > rodjenNakon);
            }

            if (search.JeVakcinisan.HasValue)
            {
                var prijeGodinu = danas.AddYears(-1);
                query = query.Where(x => x.Termini.Any(t => t.Status == StatusTermina.Zavrsen
                    && t.DatumTermina >= prijeGodinu
                    && t.StavkeTermina.Any(s => s.Usluga.Vrsta.JeVakcinacija)) == search.JeVakcinisan);
            }

            return query;
        }

        protected override IQueryable<Ljubimac> AddInclude(IQueryable<Ljubimac> query)
        {
            return query.Include(x => x.Korisnik).Include(x => x.Rasa).ThenInclude(x => x.VrstaLjubimca);
        }

        protected override async Task BeforeInsertAsync(LjubimciUpsertRequest request, Ljubimac entity, CancellationToken cancellationToken)
        {
            entity.Korisnik = await UcitajVlasnikaAsync(request.VlasnikId, cancellationToken);
            entity.Rasa = await UcitajRasuAsync(request.RasaId, cancellationToken);
        }

        protected override async Task BeforeUpdateAsync(LjubimciUpsertRequest request, Ljubimac entity, CancellationToken cancellationToken)
        {
            // Klijent ne može prebaciti ljubimca drugom vlasniku; osoblje može ispraviti vlasnika.
            if (JeOsoblje && request.VlasnikId.HasValue && request.VlasnikId != entity.KorisnikId)
            {
                entity.Korisnik = await UcitajVlasnikaAsync(request.VlasnikId, cancellationToken);
            }

            entity.Rasa = await UcitajRasuAsync(request.RasaId, cancellationToken);
        }

        protected override async Task BeforeDeleteAsync(Ljubimac entity, CancellationToken cancellationToken)
        {
            if (await Context.Termini.AnyAsync(x => x.LjubimacId == entity.LjubimacId && TerminStateMachine.AktivniStatusi.Contains(x.Status), cancellationToken))
            {
                throw new BusinessException("Ljubimca nije moguće obrisati jer ima zakazane termine. Najprije otkažite te termine.");
            }
        }

        private async Task<Korisnik> UcitajVlasnikaAsync(int? vlasnikId, CancellationToken cancellationToken)
        {
            if (!JeOsoblje)
            {
                int korisnikId = _currentUser.KorisnikId;
                return await Context.Korisnici.FirstAsync(x => x.KorisnikId == korisnikId, cancellationToken);
            }

            if (!vlasnikId.HasValue)
            {
                throw new BusinessException("Potrebno je odabrati vlasnika ljubimca.");
            }

            return await Context.Korisnici.FirstOrDefaultAsync(x => x.KorisnikId == vlasnikId
                    && x.KorisniciUloge.Any(ku => ku.Uloga.Naziv == Uloge.Klijent), cancellationToken)
                ?? throw new BusinessException("Odabrani vlasnik ne postoji ili nije registrovan kao klijent.");
        }

        private async Task<Rasa> UcitajRasuAsync(int rasaId, CancellationToken cancellationToken)
        {
            return await Context.Rase.Include(x => x.VrstaLjubimca).FirstOrDefaultAsync(x => x.RasaId == rasaId, cancellationToken)
                ?? throw new BusinessException("Odabrana rasa ne postoji.");
        }
    }
}
