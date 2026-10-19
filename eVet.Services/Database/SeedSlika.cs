using Microsoft.EntityFrameworkCore;

namespace eVet.Services.Database;

/// <summary>
/// Upisuje slike testnih podataka (usluge, vrste usluga, ljubimci, korisnici) iz datoteka u SeedSlike.
/// Slike nisu u HasData seedu da migracije ne bi sadržavale megabajte binarnih podataka.
/// Postojeće slike se ne mijenjaju, pa slika koju korisnik promijeni kroz aplikaciju ostaje sačuvana.
/// </summary>
public static class SeedSlika
{
    private const string PrefiksResursa = "eVet.Services.Database.SeedSlike.";

    public static async Task PostaviAsync(EVetDbContext context, CancellationToken cancellationToken = default)
    {
        await PostaviAsync(context.Usluge, x => x.UslugaId, "usluga", cancellationToken);
        await PostaviAsync(context.VrsteUsluga, x => x.VrstaId, "vrsta_usluge", cancellationToken);
        await PostaviAsync(context.Ljubimci, x => x.LjubimacId, "ljubimac", cancellationToken);
        await PostaviAsync(context.Korisnici, x => x.KorisnikId, "korisnik", cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task PostaviAsync<T>(DbSet<T> skup, Func<T, int> id, string prefiks, CancellationToken cancellationToken) where T : class, ISlikaEntity
    {
        var bezSlike = await skup.Where(x => x.Slika == null).ToListAsync(cancellationToken);

        foreach (var entitet in bezSlike)
        {
            var slika = ProcitajResurs($"{prefiks}_{id(entitet)}.jpg");
            if (slika != null)
            {
                entitet.Slika = slika;
            }
        }
    }

    private static byte[]? ProcitajResurs(string naziv)
    {
        using var stream = typeof(SeedSlika).Assembly.GetManifestResourceStream(PrefiksResursa + naziv);
        if (stream == null)
        {
            return null;
        }

        using var memorija = new MemoryStream();
        stream.CopyTo(memorija);
        return memorija.ToArray();
    }
}
