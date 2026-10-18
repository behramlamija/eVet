using eVet.Model.Constants;
using eVet.Model.Enums;
using eVet.Model.Exceptions;
using eVet.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services.Slike
{
    public class SlikeService : ISlikeService
    {
        private readonly EVetDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public SlikeService(EVetDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<SlikaDatoteka> GetAsync(TipSlike tip, int id, CancellationToken cancellationToken = default)
        {
            byte[]? sadrzaj = tip switch
            {
                TipSlike.Usluga => await _context.Usluge.Where(x => x.UslugaId == id).Select(x => x.Slika).FirstOrDefaultAsync(cancellationToken),
                TipSlike.VrstaUsluge => await _context.VrsteUsluga.Where(x => x.VrstaId == id).Select(x => x.Slika).FirstOrDefaultAsync(cancellationToken),
                TipSlike.Ljubimac => await DozvoljeniLjubimci().Where(x => x.LjubimacId == id).Select(x => x.Slika).FirstOrDefaultAsync(cancellationToken),
                TipSlike.Korisnik => await VidljiviKorisnici().Where(x => x.KorisnikId == id).Select(x => x.Slika).FirstOrDefaultAsync(cancellationToken),
                _ => null
            };

            if (sadrzaj == null)
            {
                throw new NotFoundException("Slika ne postoji.");
            }

            return new SlikaDatoteka(sadrzaj, SlikaValidator.OdrediContentType(sadrzaj));
        }

        public async Task SetAsync(TipSlike tip, int id, byte[] sadrzaj, string? contentType, CancellationToken cancellationToken = default)
        {
            SlikaValidator.Validiraj(sadrzaj, contentType);

            var entity = await GetEntitetZaIzmjenuAsync(tip, id, cancellationToken);
            entity.Slika = sadrzaj;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(TipSlike tip, int id, CancellationToken cancellationToken = default)
        {
            var entity = await GetEntitetZaIzmjenuAsync(tip, id, cancellationToken);
            entity.Slika = null;

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<ISlikaEntity> GetEntitetZaIzmjenuAsync(TipSlike tip, int id, CancellationToken cancellationToken)
        {
            bool jeVlasnik = _currentUser.IsInRole(Uloge.Vlasnik);

            ISlikaEntity? entity = tip switch
            {
                TipSlike.Usluga when jeVlasnik => await _context.Usluge.FirstOrDefaultAsync(x => x.UslugaId == id, cancellationToken),
                TipSlike.VrstaUsluge when jeVlasnik => await _context.VrsteUsluga.FirstOrDefaultAsync(x => x.VrstaId == id, cancellationToken),
                TipSlike.Ljubimac => await DozvoljeniLjubimci().FirstOrDefaultAsync(x => x.LjubimacId == id, cancellationToken),
                TipSlike.Korisnik when jeVlasnik || id == _currentUser.KorisnikId => await _context.Korisnici.FirstOrDefaultAsync(x => x.KorisnikId == id, cancellationToken),
                TipSlike.Usluga or TipSlike.VrstaUsluge or TipSlike.Korisnik => throw new ForbiddenException("Nemate pravo mijenjati ovu sliku."),
                _ => null
            };

            return entity ?? throw new NotFoundException("Zapis kojem pripada slika ne postoji.");
        }

        // Klijent vidi samo svoje ljubimce, osoblje stanice vidi sve.
        private IQueryable<Ljubimac> DozvoljeniLjubimci()
        {
            if (!JeOsoblje())
            {
                int korisnikId = _currentUser.KorisnikId;
                return _context.Ljubimci.Where(x => x.KorisnikId == korisnikId);
            }

            return _context.Ljubimci;
        }

        // Klijent vidi svoju sliku i slike osoblja stanice, osoblje vidi sve korisnike.
        private IQueryable<Korisnik> VidljiviKorisnici()
        {
            if (JeOsoblje())
            {
                return _context.Korisnici;
            }

            int korisnikId = _currentUser.KorisnikId;
            return _context.Korisnici.Where(x => x.KorisnikId == korisnikId
                || x.KorisniciUloge.Any(ku => ku.Uloga.Naziv == Uloge.Veterinar || ku.Uloga.Naziv == Uloge.Vlasnik));
        }

        private bool JeOsoblje() => _currentUser.IsInRole(Uloge.Vlasnik) || _currentUser.IsInRole(Uloge.Veterinar);
    }
}
