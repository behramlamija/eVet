namespace eVet.Services.Security
{
    public static class JwtClaimNames
    {
        public const string KorisnikId = "sub";
        public const string KorisnickoIme = "name";
        public const string Uloga = "role";
        public const string SecurityStamp = "stamp";
    }
}
