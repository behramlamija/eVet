using eVet.Model;
using eVet.Model.Constants;
using eVet.Model.Enums;
using eVet.Model.SearchObjects;
using eVet.Model.Statistika;
using eVet.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    // Svaki pokazatelj se računa jednim agregatnim (GroupBy) upitom u bazi.
    public class StatistikaService : IStatistikaService
    {
        private readonly EVetDbContext _context;

        public StatistikaService(EVetDbContext context)
        {
            _context = context;
        }

        public async Task<StatistikaPoslovanja> GetPoslovanjeAsync(DateOnly? datumOd, DateOnly? datumDo, CancellationToken cancellationToken = default)
        {
            var termini = FiltrirajTermine(_context.Termini.AsNoTracking(), datumOd, datumDo);
            var zavrseni = termini.Where(x => x.Status == StatusTermina.Zavrsen);
            var stavkeZavrsenih = _context.StavkeTermina.AsNoTracking().Where(x => zavrseni.Contains(x.Termin));

            var sumarno = await termini
                .GroupBy(x => 1)
                .Select(g => new
                {
                    Ukupno = g.Count(),
                    Zavrseni = g.Count(x => x.Status == StatusTermina.Zavrsen),
                    Otkazani = g.Count(x => x.Status == StatusTermina.Otkazan || x.Status == StatusTermina.Odbijen),
                    Prihod = g.Sum(x => x.Status == StatusTermina.Zavrsen ? x.UkupnaCijena : 0)
                })
                .FirstOrDefaultAsync(cancellationToken);

            var usluge = await stavkeZavrsenih
                .GroupBy(x => 1)
                .Select(g => new
                {
                    Ukupno = g.Count(),
                    Operacije = g.Count(x => x.Usluga.Vrsta.JeOperacija)
                })
                .FirstOrDefaultAsync(cancellationToken);

            var najcescaUsluga = await stavkeZavrsenih
                .GroupBy(x => x.Usluga.Naziv)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync(cancellationToken);

            var najzastupljenijaRasa = await termini
                .GroupBy(x => x.Ljubimac.Rasa.Naziv)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync(cancellationToken);

            var topVeterinar = await zavrseni
                .GroupBy(x => new { x.Veterinar.Ime, x.Veterinar.Prezime })
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key.Ime + " " + g.Key.Prezime)
                .FirstOrDefaultAsync(cancellationToken);

            var noviKlijenti = _context.Korisnici.AsNoTracking()
                .Where(x => x.KorisniciUloge.Any(ku => ku.Uloga.Naziv == Uloge.Klijent));
            if (datumOd.HasValue)
            {
                var od = datumOd.Value.ToDateTime(TimeOnly.MinValue);
                noviKlijenti = noviKlijenti.Where(x => x.DatumRegistracije >= od);
            }
            if (datumDo.HasValue)
            {
                var @do = datumDo.Value.AddDays(1).ToDateTime(TimeOnly.MinValue);
                noviKlijenti = noviKlijenti.Where(x => x.DatumRegistracije < @do);
            }

            return new StatistikaPoslovanja
            {
                BrojTermina = sumarno?.Ukupno ?? 0,
                BrojZavrsenihTermina = sumarno?.Zavrseni ?? 0,
                BrojOtkazanihTermina = sumarno?.Otkazani ?? 0,
                UkupniPrihod = sumarno?.Prihod ?? 0,
                BrojPruzenihUsluga = usluge?.Ukupno ?? 0,
                BrojOperacija = usluge?.Operacije ?? 0,
                BrojNovihKlijenata = await noviKlijenti.CountAsync(cancellationToken),
                NajcescaUsluga = najcescaUsluga,
                NajzastupljenijaRasa = najzastupljenijaRasa,
                TopVeterinar = topVeterinar
            };
        }

        public async Task<List<StatistikaPeriod>> GetPoPeriodimaAsync(GrupisanjeStatistike grupisanje, int? godina, CancellationToken cancellationToken = default)
        {
            bool poMjesecima = grupisanje == GrupisanjeStatistike.Mjesec;

            var termini = _context.Termini.AsNoTracking();
            if (godina.HasValue)
            {
                termini = termini.Where(x => x.DatumTermina.Year == godina);
            }

            var poTerminima = await termini
                .GroupBy(x => new { x.DatumTermina.Year, Month = poMjesecima ? x.DatumTermina.Month : 0 })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    BrojTermina = g.Count(),
                    Prihod = g.Sum(x => x.Status == StatusTermina.Zavrsen ? x.UkupnaCijena : 0)
                })
                .ToListAsync(cancellationToken);

            var poOperacijama = await _context.StavkeTermina.AsNoTracking()
                .Where(x => termini.Contains(x.Termin) && x.Termin.Status == StatusTermina.Zavrsen && x.Usluga.Vrsta.JeOperacija)
                .GroupBy(x => new { x.Termin.DatumTermina.Year, Month = poMjesecima ? x.Termin.DatumTermina.Month : 0 })
                .Select(g => new { g.Key.Year, g.Key.Month, Broj = g.Count() })
                .ToListAsync(cancellationToken);

            return poTerminima
                .Select(x => new StatistikaPeriod
                {
                    Godina = x.Year,
                    Mjesec = poMjesecima ? x.Month : null,
                    BrojTermina = x.BrojTermina,
                    Prihod = x.Prihod,
                    BrojOperacija = poOperacijama.FirstOrDefault(o => o.Year == x.Year && o.Month == x.Month)?.Broj ?? 0
                })
                .OrderBy(x => x.Godina)
                .ThenBy(x => x.Mjesec)
                .ToList();
        }

        public async Task<PagedResult<StatistikaVeterinara>> GetVeterinareAsync(KorisnikOsnovnoSearchObject search, CancellationToken cancellationToken = default)
        {
            var veterinari = _context.Korisnici.AsNoTracking()
                .Where(x => x.KorisniciUloge.Any(ku => ku.Uloga.Naziv == Uloge.Veterinar));

            if (!string.IsNullOrWhiteSpace(search.FTS))
            {
                veterinari = veterinari.Where(x => x.Ime.Contains(search.FTS) || x.Prezime.Contains(search.FTS));
            }

            int page = Math.Max(search.Page ?? 0, 0);
            int pageSize = Math.Clamp(search.PageSize ?? BaseSearchObject.DefaultPageSize, 1, BaseSearchObject.MaxPageSize);
            int count = await veterinari.CountAsync(cancellationToken);

            // Brojevi se računaju kao podupiti u istom SQL upitu (bez N+1).
            var lista = await veterinari
                .Select(x => new StatistikaVeterinara
                {
                    KorisnikId = x.KorisnikId,
                    Ime = x.Ime,
                    Prezime = x.Prezime,
                    ImaSliku = x.Slika != null,
                    SpecijalnostNaziv = x.Specijalnost != null ? x.Specijalnost.Naziv : null,
                    JeAktivan = x.JeAktivan,
                    BrojTermina = x.TerminiVeterinara.Count(),
                    BrojZavrsenihTermina = x.TerminiVeterinara.Count(t => t.Status == StatusTermina.Zavrsen),
                    PozitivneRecenzije = x.TerminiVeterinara.Count(t => t.Recenzija != null && t.Recenzija.VeterinarPozitivan),
                    NegativneRecenzije = x.TerminiVeterinara.Count(t => t.Recenzija != null && !t.Recenzija.VeterinarPozitivan)
                })
                .OrderByDescending(x => x.BrojTermina)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<StatistikaVeterinara> { Count = count, ResultList = lista };
        }

        private static IQueryable<Termin> FiltrirajTermine(IQueryable<Termin> query, DateOnly? datumOd, DateOnly? datumDo)
        {
            if (datumOd.HasValue)
            {
                query = query.Where(x => x.DatumTermina >= datumOd);
            }

            if (datumDo.HasValue)
            {
                query = query.Where(x => x.DatumTermina <= datumDo);
            }

            return query;
        }
    }
}
