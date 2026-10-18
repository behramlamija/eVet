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
    public class RecenzijeService : BaseService<Model.Recenzije, RecenzijeSearchObject, Recenzija>, IRecenzijeService
    {
        private readonly ICurrentUserService _currentUser;

        public RecenzijeService(EVetDbContext context, IMapper mapper, ICurrentUserService currentUser) : base(context, mapper)
        {
            _currentUser = currentUser;
        }

        protected override string NazivEntiteta => "Recenzija";

        protected override IQueryable<Recenzija> AddFilter(RecenzijeSearchObject search, IQueryable<Recenzija> query)
        {
            if (search.VeterinarId.HasValue)
            {
                query = query.Where(x => x.Termin.VeterinarId == search.VeterinarId);
            }

            if (search.UslugaId.HasValue)
            {
                query = query.Where(x => x.Termin.StavkeTermina.Any(s => s.UslugaId == search.UslugaId));
            }

            if (search.VeterinarPozitivan.HasValue)
            {
                query = query.Where(x => x.VeterinarPozitivan == search.VeterinarPozitivan);
            }

            return query;
        }

        public async Task<Model.Recenzije> InsertAsync(RecenzijaInsertRequest request, CancellationToken cancellationToken = default)
        {
            int klijentId = _currentUser.KorisnikId;

            var termin = await Context.Termini
                .Include(x => x.Veterinar)
                .Include(x => x.Korisnik)
                .FirstOrDefaultAsync(x => x.TerminId == request.TerminId && x.KorisnikId == klijentId, cancellationToken)
                ?? throw new NotFoundException("Termin ne postoji ili ne pripada vama.");

            if (termin.Status != StatusTermina.Zavrsen)
            {
                throw new BusinessException("Recenziju je moguće ostaviti tek nakon što je termin završen.");
            }

            if (await Context.Recenzije.AnyAsync(x => x.TerminId == termin.TerminId, cancellationToken))
            {
                throw new BusinessException("Za ovaj termin ste već ostavili recenziju.");
            }

            var recenzija = new Recenzija
            {
                Korisnik = termin.Korisnik,
                Termin = termin,
                UslugaPozitivna = request.UslugaPozitivna,
                VeterinarPozitivan = request.VeterinarPozitivan,
                Komentar = request.Komentar,
                DatumDodavanja = DateTime.UtcNow
            };

            Context.Recenzije.Add(recenzija);
            NotifikacijeHelper.Dodaj(Context, termin.VeterinarId, "Nova recenzija",
                $"{termin.Korisnik.Ime} {termin.Korisnik.Prezime} je ocijenio/la termin {termin.Sifra}.");

            await Context.SaveChangesAsync(cancellationToken);

            return Mapper.Map<Model.Recenzije>(recenzija);
        }

        public async Task<Model.Recenzije> OdgovoriAsync(int id, RecenzijaOdgovorRequest request, CancellationToken cancellationToken = default)
        {
            int korisnikId = _currentUser.KorisnikId;

            var recenzija = await Context.Recenzije
                .Include(x => x.Korisnik)
                .Include(x => x.Termin).ThenInclude(x => x.Veterinar)
                .Include(x => x.RecenzijaOdgovori).ThenInclude(x => x.Korisnik)
                .FirstOrDefaultAsync(x => x.RecenzijaId == id, cancellationToken)
                ?? throw new NotFoundException("Recenzija ne postoji.");

            if (!_currentUser.IsInRole(Uloge.Vlasnik) && recenzija.Termin.VeterinarId != korisnikId)
            {
                throw new ForbiddenException("Na recenziju može odgovoriti vlasnik stanice ili veterinar kojem je recenzija upućena.");
            }

            recenzija.RecenzijaOdgovori.Add(new RecenzijaOdgovor
            {
                Korisnik = await Context.Korisnici.FirstAsync(x => x.KorisnikId == korisnikId, cancellationToken),
                Komentar = request.Komentar,
                DatumDodavanja = DateTime.UtcNow
            });

            NotifikacijeHelper.Dodaj(Context, recenzija.KorisnikId, "Odgovor na vašu recenziju",
                $"Stanica je odgovorila na vašu recenziju termina {recenzija.Termin.Sifra}.");

            await Context.SaveChangesAsync(cancellationToken);

            return Mapper.Map<Model.Recenzije>(recenzija);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var recenzija = await Context.Recenzije.FirstOrDefaultAsync(x => x.RecenzijaId == id, cancellationToken)
                ?? throw new NotFoundException("Recenzija ne postoji.");

            if (!_currentUser.IsInRole(Uloge.Vlasnik) && recenzija.KorisnikId != _currentUser.KorisnikId)
            {
                throw new ForbiddenException("Recenziju može obrisati samo njen autor ili vlasnik stanice.");
            }

            recenzija.IsDeleted = true;
            recenzija.VrijemeBrisanja = DateTime.UtcNow;
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}
