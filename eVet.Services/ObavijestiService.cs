using eVet.Model.Exceptions;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    // Sistemske notifikacije trenutnog korisnika. Klijentske aplikacije periodično pozivaju broj nepročitanih (polling).
    public class ObavijestiService : BaseService<Model.Obavijesti, ObavijestiSearchObject, Obavijest>, IObavijestiService
    {
        private readonly ICurrentUserService _currentUser;

        public ObavijestiService(EVetDbContext context, IMapper mapper, ICurrentUserService currentUser) : base(context, mapper)
        {
            _currentUser = currentUser;
        }

        protected override string NazivEntiteta => "Obavijest";

        protected override IQueryable<Obavijest> ApplyPristup(IQueryable<Obavijest> query)
        {
            int korisnikId = _currentUser.KorisnikId;
            return query.Where(x => x.KorisnikId == korisnikId);
        }

        protected override IQueryable<Obavijest> AddFilter(ObavijestiSearchObject search, IQueryable<Obavijest> query)
        {
            if (search.JePogledana.HasValue)
            {
                query = query.Where(x => x.JePogledana == search.JePogledana);
            }

            return query;
        }

        public Task<int> GetBrojNeprocitanihAsync(CancellationToken cancellationToken = default)
        {
            return ApplyPristup(Context.Obavijesti).CountAsync(x => !x.JePogledana, cancellationToken);
        }

        public async Task OznaciProcitanoAsync(int id, CancellationToken cancellationToken = default)
        {
            var obavijest = await ApplyPristup(Context.Obavijesti).FirstOrDefaultAsync(x => x.ObavijestId == id, cancellationToken)
                ?? throw new NotFoundException("Obavijest ne postoji.");

            obavijest.JePogledana = true;
            await Context.SaveChangesAsync(cancellationToken);
        }

        public Task OznaciSveProcitanoAsync(CancellationToken cancellationToken = default)
        {
            return ApplyPristup(Context.Obavijesti)
                .Where(x => !x.JePogledana)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.JePogledana, true), cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var obavijest = await ApplyPristup(Context.Obavijesti).FirstOrDefaultAsync(x => x.ObavijestId == id, cancellationToken)
                ?? throw new NotFoundException("Obavijest ne postoji.");

            obavijest.IsDeleted = true;
            obavijest.VrijemeBrisanja = DateTime.UtcNow;
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}
