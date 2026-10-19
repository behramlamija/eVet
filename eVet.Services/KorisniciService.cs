using eVet.Model.Constants;
using eVet.Model.Exceptions;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using eVet.Services.Security;
using eVet.Services.Termini;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace eVet.Services
{
    public class KorisniciService : BaseCRUDService<Model.Korisnici, KorisniciSearchObject, Korisnik, KorisniciInsertRequest, KorisniciUpdateRequest>, IKorisniciService
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IMemoryCache _cache;
        private readonly INotifikacijeService _notifikacije;

        public KorisniciService(EVetDbContext context, IMapper mapper, ICurrentUserService currentUser, IMemoryCache cache, INotifikacijeService notifikacije) : base(context, mapper)
        {
            _notifikacije = notifikacije;
            _currentUser = currentUser;
            _cache = cache;
        }

        protected override string NazivEntiteta => "Korisnik";

        protected override IQueryable<Korisnik> AddFilter(KorisniciSearchObject search, IQueryable<Korisnik> query)
        {
            if (!string.IsNullOrWhiteSpace(search.ImeGTE))
            {
                query = query.Where(x => x.Ime.StartsWith(search.ImeGTE));
            }

            if (!string.IsNullOrWhiteSpace(search.PrezimeGTE))
            {
                query = query.Where(x => x.Prezime.StartsWith(search.PrezimeGTE));
            }

            if (!string.IsNullOrWhiteSpace(search.Email))
            {
                query = query.Where(x => x.Email.StartsWith(search.Email));
            }

            if (!string.IsNullOrWhiteSpace(search.KorisnickoIme))
            {
                query = query.Where(x => x.KorisnickoIme.StartsWith(search.KorisnickoIme));
            }

            if (search.UlogaId.HasValue)
            {
                query = query.Where(x => x.KorisniciUloge.Any(ku => ku.UlogaId == search.UlogaId));
            }

            if (search.SpecijalnostId.HasValue)
            {
                query = query.Where(x => x.SpecijalnostId == search.SpecijalnostId);
            }

            if (search.JeAktivan.HasValue)
            {
                query = query.Where(x => x.JeAktivan == search.JeAktivan);
            }

            return query;
        }

        protected override IQueryable<Korisnik> AddInclude(IQueryable<Korisnik> query)
        {
            return query
                .Include(x => x.KorisniciUloge).ThenInclude(x => x.Uloga)
                .Include(x => x.GradRodjenja)
                .Include(x => x.Specijalnost);
        }

        protected override async Task BeforeInsertAsync(KorisniciInsertRequest request, Korisnik entity, CancellationToken cancellationToken)
        {
            await ProvjeriJedinstvenostAsync(request.KorisnickoIme, request.Email, null, cancellationToken);
            await PostaviUlogeAsync(entity, request.UlogeIds, cancellationToken);
            await PostaviPodatkeZaposlenjaAsync(entity, request.GradRodjenjaId, request.SpecijalnostId, cancellationToken);
            entity.PostaviLozinku(request.Lozinka);
            entity.DatumRegistracije = DateTime.UtcNow;
        }

        protected override async Task BeforeUpdateAsync(KorisniciUpdateRequest request, Korisnik entity, CancellationToken cancellationToken)
        {
            await ProvjeriJedinstvenostAsync(null, request.Email, entity.KorisnikId, cancellationToken);

            if (entity.KorisnikId == _currentUser.KorisnikId)
            {
                if (!request.JeAktivan)
                {
                    throw new BusinessException("Ne možete deaktivirati vlastiti korisnički račun.");
                }

                var vlasnikUloga = await Context.Uloge.FirstAsync(u => u.Naziv == Uloge.Vlasnik, cancellationToken);
                if (!request.UlogeIds.Contains(vlasnikUloga.UlogaId))
                {
                    throw new BusinessException("Ne možete sebi ukloniti ulogu vlasnika.");
                }
            }

            await PostaviUlogeAsync(entity, request.UlogeIds, cancellationToken);
            await PostaviPodatkeZaposlenjaAsync(entity, request.GradRodjenjaId, request.SpecijalnostId, cancellationToken);

            bool ponistiPrijave = !request.JeAktivan;

            if (!string.IsNullOrEmpty(request.Lozinka))
            {
                entity.PostaviLozinku(request.Lozinka);
                ponistiPrijave = true;
            }

            if (ponistiPrijave)
            {
                entity.PonistiPrijave();
                SecurityStampCache.Ponisti(_cache, entity.KorisnikId);
            }
        }

        protected override async Task BeforeDeleteAsync(Korisnik entity, CancellationToken cancellationToken)
        {
            if (entity.KorisnikId == _currentUser.KorisnikId)
            {
                throw new BusinessException("Ne možete obrisati vlastiti korisnički račun.");
            }

            var danas = DateOnly.FromDateTime(DateTime.UtcNow);
            int brojAktivnihTermina = await Context.Termini.CountAsync(t =>
                (t.VeterinarId == entity.KorisnikId || t.KorisnikId == entity.KorisnikId)
                && TerminStateMachine.AktivniStatusi.Contains(t.Status)
                && t.DatumTermina >= danas, cancellationToken);

            if (brojAktivnihTermina > 0)
            {
                throw new BusinessException($"Korisnika nije moguće obrisati jer ima aktivnih termina ({brojAktivnihTermina}). Termine je potrebno najprije završiti ili otkazati.");
            }

            entity.PonistiPrijave();
            SecurityStampCache.Ponisti(_cache, entity.KorisnikId);
        }

        public Task<Model.PagedResult<Model.KorisnikOsnovno>> GetPoUloziAsync(string uloga, KorisnikOsnovnoSearchObject search, CancellationToken cancellationToken = default)
        {
            var query = Context.Korisnici.AsNoTracking()
                .Where(x => x.JeAktivan && x.KorisniciUloge.Any(ku => ku.Uloga.Naziv == uloga));

            if (!string.IsNullOrWhiteSpace(search.FTS))
            {
                query = query.Where(x => x.Ime.Contains(search.FTS) || x.Prezime.Contains(search.FTS) || x.Email.Contains(search.FTS));
            }

            return query
                .OrderBy(x => x.Prezime).ThenBy(x => x.Ime)
                .ToPagedResultAsync<Korisnik, Model.KorisnikOsnovno>(query, search, Mapper.Config, cancellationToken);
        }

        public async Task<Model.Korisnici> RegistracijaAsync(RegistracijaRequest request, CancellationToken cancellationToken = default)
        {
            await ProvjeriJedinstvenostAsync(request.KorisnickoIme, request.Email, null, cancellationToken);

            var entity = Mapper.Map<Korisnik>(request);
            var klijentUloga = await Context.Uloge.FirstAsync(u => u.Naziv == Uloge.Klijent, cancellationToken);
            entity.KorisniciUloge.Add(new KorisnikUloga { Uloga = klijentUloga });
            entity.PostaviLozinku(request.Lozinka);
            entity.JeAktivan = true;
            entity.DatumRegistracije = DateTime.UtcNow;

            Context.Korisnici.Add(entity);
            _notifikacije.Dodaj(entity, "Dobrodošli u PetCity",
                $"Poštovani/a {entity.Ime}, vaš korisnički račun ({entity.KorisnickoIme}) je uspješno kreiran. Sada možete zakazivati termine za svoje ljubimce.");
            await Context.SaveChangesAsync(cancellationToken);
            await _notifikacije.PosaljiEmailoveAsync(cancellationToken);

            return Mapper.Map<Model.Korisnici>(entity);
        }

        public Task<Model.Korisnici> GetProfilAsync(CancellationToken cancellationToken = default)
        {
            return GetByIdAsync(_currentUser.KorisnikId, cancellationToken);
        }

        public async Task<Model.Korisnici> UpdateProfilAsync(ProfilUpdateRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await GetEntityForUpdateAsync(_currentUser.KorisnikId, cancellationToken);

            await ProvjeriJedinstvenostAsync(null, request.Email, entity.KorisnikId, cancellationToken);

            Mapper.Map(request, entity);
            await Context.SaveChangesAsync(cancellationToken);

            return Mapper.Map<Model.Korisnici>(entity);
        }

        public async Task PromijeniLozinkuAsync(PromjenaLozinkeRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await GetEntityForUpdateAsync(_currentUser.KorisnikId, cancellationToken);

            if (!entity.ProvjeriLozinku(request.StaraLozinka))
            {
                throw new BusinessException("Trenutna lozinka nije ispravna.");
            }

            entity.PostaviLozinku(request.NovaLozinka);
            await Context.SaveChangesAsync(cancellationToken);
        }

        private async Task ProvjeriJedinstvenostAsync(string? korisnickoIme, string email, int? izuzmiKorisnikId, CancellationToken cancellationToken)
        {
            var postojeci = Context.Korisnici.Where(k => k.KorisnikId != izuzmiKorisnikId);

            if (korisnickoIme != null && await postojeci.AnyAsync(k => k.KorisnickoIme == korisnickoIme, cancellationToken))
            {
                throw new BusinessException("Korisničko ime je već zauzeto. Odaberite drugo korisničko ime.");
            }

            if (await postojeci.AnyAsync(k => k.Email == email, cancellationToken))
            {
                throw new BusinessException("Email adresa je već registrovana.");
            }
        }

        private async Task PostaviUlogeAsync(Korisnik entity, List<int> ulogeIds, CancellationToken cancellationToken)
        {
            var trazeneUloge = ulogeIds.Distinct().ToList();
            var uloge = await Context.Uloge.Where(u => trazeneUloge.Contains(u.UlogaId)).ToListAsync(cancellationToken);

            if (uloge.Count != trazeneUloge.Count)
            {
                throw new BusinessException("Odabrana uloga ne postoji.");
            }

            foreach (var postojeca in entity.KorisniciUloge.Where(ku => !trazeneUloge.Contains(ku.UlogaId)).ToList())
            {
                entity.KorisniciUloge.Remove(postojeca);
                Context.KorisniciUloge.Remove(postojeca);
            }

            foreach (var uloga in uloge.Where(u => entity.KorisniciUloge.All(ku => ku.UlogaId != u.UlogaId)))
            {
                entity.KorisniciUloge.Add(new KorisnikUloga { Uloga = uloga, UlogaId = uloga.UlogaId });
            }
        }

        private async Task PostaviPodatkeZaposlenjaAsync(Korisnik entity, int? gradRodjenjaId, int? specijalnostId, CancellationToken cancellationToken)
        {
            entity.GradRodjenja = gradRodjenjaId.HasValue
                ? await Context.Gradovi.FirstOrDefaultAsync(x => x.GradId == gradRodjenjaId, cancellationToken)
                    ?? throw new BusinessException("Odabrani grad rođenja ne postoji.")
                : null;

            entity.Specijalnost = specijalnostId.HasValue
                ? await Context.Specijalnosti.FirstOrDefaultAsync(x => x.SpecijalnostId == specijalnostId, cancellationToken)
                    ?? throw new BusinessException("Odabrana specijalnost ne postoji.")
                : null;
        }
    }
}
