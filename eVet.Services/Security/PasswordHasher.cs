using System.Security.Cryptography;
using System.Text;

namespace eVet.Services.Security
{
    // PBKDF2 (SHA-256). Isti format koriste i seed podaci i runtime registracija.
    public static class PasswordHasher
    {
        private const int Iterations = 100_000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public static string GenerateSalt()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(SaltSize));
        }

        public static string Hash(string lozinka, string salt)
        {
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(lozinka),
                Convert.FromBase64String(salt),
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            return Convert.ToBase64String(hash);
        }

        public static bool Verify(string lozinka, string salt, string ocekivaniHash)
        {
            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(Hash(lozinka, salt)),
                Convert.FromBase64String(ocekivaniHash));
        }
    }
}
