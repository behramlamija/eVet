using Microsoft.Extensions.Caching.Memory;

namespace eVet.Services.Security
{
    public static class SecurityStampCache
    {
        public static readonly TimeSpan Trajanje = TimeSpan.FromMinutes(5);

        public static string Kljuc(int korisnikId) => $"security-stamp:{korisnikId}";

        public static void Ponisti(IMemoryCache cache, int korisnikId) => cache.Remove(Kljuc(korisnikId));
    }
}
