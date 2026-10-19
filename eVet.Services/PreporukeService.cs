using eVet.Model;
using eVet.Model.Enums;
using eVet.Model.Exceptions;
using eVet.Services.Database;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    /// <summary>
    /// Item-item collaborative filtering nad uslugama: usluga B se preporučuje uz uslugu A ako su ih klijenti
    /// rezervisali zajedno u istom terminu. Detaljan opis je u recommender-dokumentacija.md.
    /// </summary>
    public class PreporukeService : IPreporukeService
    {
        public const int MaksimalnoPreporuka = 5;
        public const int MaksimalnoPolaznihUsluga = 20;

        // Otkazani i odbijeni termini nisu stvarno korištena kombinacija usluga, pa ne ulaze u preporuke.
        private static readonly StatusTermina[] IskljuceniStatusi = { StatusTermina.Otkazan, StatusTermina.Odbijen };

        private readonly EVetDbContext _context;

        public PreporukeService(EVetDbContext context)
        {
            _context = context;
        }

        public async Task<List<PreporukeUsluga>> GetPreporukeAsync(IReadOnlyCollection<int> uslugeIds, CancellationToken cancellationToken = default)
        {
            var polazne = uslugeIds.Distinct().ToList();
            if (polazne.Count == 0)
            {
                throw new BusinessException("Odaberite barem jednu uslugu za koju se traže preporuke.");
            }

            if (polazne.Count > MaksimalnoPolaznihUsluga)
            {
                throw new BusinessException($"Preporuke se mogu tražiti za najviše {MaksimalnoPolaznihUsluga} usluga odjednom.");
            }

            var stavke = _context.StavkeTermina.Where(x => !IskljuceniStatusi.Contains(x.Termin.Status));

            // Za svaku polaznu uslugu A: u koliko je termina rezervisana (podrška A).
            var brojTerminaIzvora = await stavke
                .Where(x => polazne.Contains(x.UslugaId))
                .GroupBy(x => x.UslugaId)
                .Select(g => new { UslugaId = g.Key, Broj = g.Select(x => x.TerminId).Distinct().Count() })
                .ToDictionaryAsync(x => x.UslugaId, x => x.Broj, cancellationToken);

            if (brojTerminaIzvora.Count == 0)
            {
                return new List<PreporukeUsluga>();
            }

            // Za svaki par (A, B): u koliko je termina B rezervisana zajedno s A (zajednička podrška).
            var parovi = await (
                from a in stavke
                join b in stavke on a.TerminId equals b.TerminId
                where polazne.Contains(a.UslugaId) && !polazne.Contains(b.UslugaId)
                group a.TerminId by new { IzvorId = a.UslugaId, CiljId = b.UslugaId } into g
                select new { g.Key.IzvorId, g.Key.CiljId, Broj = g.Distinct().Count() })
                .ToListAsync(cancellationToken);

            // Ako je kandidat povezan s više polaznih usluga, uzima se najjača veza
            // (najveća pouzdanost, pa veći broj zajedničkih termina) i ona se navodi u objašnjenju.
            var najbolje = parovi
                .Select(p => new { p.IzvorId, p.CiljId, p.Broj, Pouzdanost = (decimal)p.Broj / brojTerminaIzvora[p.IzvorId] })
                .GroupBy(p => p.CiljId)
                .Select(g => g.OrderByDescending(p => p.Pouzdanost).ThenByDescending(p => p.Broj).ThenBy(p => p.IzvorId).First())
                .OrderByDescending(p => p.Pouzdanost)
                .ThenByDescending(p => p.Broj)
                .ThenBy(p => p.CiljId)
                .ToList();

            var potrebneUsluge = najbolje.Select(p => p.CiljId).Concat(najbolje.Select(p => p.IzvorId)).Distinct().ToList();

            // Globalni filter izbacuje obrisane usluge, pa se one ne mogu ni preporučiti.
            var usluge = await _context.Usluge
                .Where(x => potrebneUsluge.Contains(x.UslugaId))
                .ProjectToType<Usluge>()
                .ToDictionaryAsync(x => x.UslugaId, cancellationToken);

            return najbolje
                .Where(p => usluge.ContainsKey(p.CiljId) && usluge.ContainsKey(p.IzvorId))
                .Take(MaksimalnoPreporuka)
                .Select(p =>
                {
                    var izvor = usluge[p.IzvorId];
                    var ukupno = brojTerminaIzvora[p.IzvorId];
                    return new PreporukeUsluga
                    {
                        Usluga = usluge[p.CiljId],
                        UslugaIzvorId = izvor.UslugaId,
                        UslugaIzvorNaziv = izvor.Naziv,
                        BrojZajednickihTermina = p.Broj,
                        BrojTerminaIzvora = ukupno,
                        Pouzdanost = Math.Round(p.Pouzdanost, 2),
                        Objasnjenje = $"Klijenti koji rezervišu uslugu \"{izvor.Naziv}\" uz nju su rezervisali i ovu uslugu u {p.Broj} od {ukupno} termina ({p.Pouzdanost:P0})."
                    };
                })
                .ToList();
        }
    }
}
