using eVet.Services.Database;

namespace eVet.Services
{
    // Notifikacije se dodaju u isti DbContext kao i poslovna promjena, pa se snimaju u istoj transakciji.
    public static class NotifikacijeHelper
    {
        public static void Dodaj(EVetDbContext context, int korisnikId, string naslov, string sadrzaj)
        {
            context.Obavijesti.Add(new Obavijest
            {
                KorisnikId = korisnikId,
                Naslov = naslov,
                Sadrzaj = sadrzaj,
                DatumObavijesti = DateTime.UtcNow
            });
        }
    }
}
