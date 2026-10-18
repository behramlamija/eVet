using eVet.Model.Exceptions;

namespace eVet.Services.Slike
{
    // Tip slike se određuje iz sadržaja (magic bytes), a ne iz ekstenzije ili zaglavlja koje šalje klijent.
    public static class SlikaValidator
    {
        public const int MaksimalnaVelicina = 2 * 1024 * 1024;

        private static readonly byte[] Jpeg = { 0xFF, 0xD8, 0xFF };
        private static readonly byte[] Png = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        private static readonly byte[] Riff = { 0x52, 0x49, 0x46, 0x46 };
        private static readonly byte[] Webp = { 0x57, 0x45, 0x42, 0x50 };

        public static string OdrediContentType(byte[] sadrzaj)
        {
            if (sadrzaj.AsSpan().StartsWith(Jpeg))
            {
                return "image/jpeg";
            }

            if (sadrzaj.AsSpan().StartsWith(Png))
            {
                return "image/png";
            }

            if (sadrzaj.Length > 12 && sadrzaj.AsSpan().StartsWith(Riff) && sadrzaj.AsSpan(8, 4).SequenceEqual(Webp))
            {
                return "image/webp";
            }

            return "application/octet-stream";
        }

        public static void Validiraj(byte[] sadrzaj, string? deklarisaniContentType)
        {
            if (sadrzaj.Length == 0)
            {
                throw new BusinessException("Odabrana datoteka je prazna.");
            }

            if (sadrzaj.Length > MaksimalnaVelicina)
            {
                throw new BusinessException("Slika može biti velika najviše 2 MB.");
            }

            var stvarniTip = OdrediContentType(sadrzaj);

            if (stvarniTip == "application/octet-stream")
            {
                throw new BusinessException("Dozvoljeni formati slike su JPG, PNG i WEBP.");
            }

            if (!string.Equals(stvarniTip, deklarisaniContentType, StringComparison.OrdinalIgnoreCase)
                && !(stvarniTip == "image/jpeg" && string.Equals(deklarisaniContentType, "image/jpg", StringComparison.OrdinalIgnoreCase)))
            {
                throw new BusinessException("Sadržaj datoteke ne odgovara navedenom tipu slike.");
            }
        }
    }
}
