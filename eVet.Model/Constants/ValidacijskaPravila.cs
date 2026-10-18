namespace eVet.Model.Constants
{
    public static class ValidacijskaPravila
    {
        public const string LozinkaRegex = @"^(?=.*[A-Za-z])(?=.*\d).{8,100}$";
        public const string LozinkaPoruka = "Lozinka mora imati 8-100 znakova, uključujući barem jedno slovo i jedan broj.";

        public const string TelefonRegex = @"^\+?[0-9][0-9 /-]{5,19}$";
        public const string TelefonPoruka = "Unesite validan broj telefona, npr. +387 61 123 456 ili 061/123-456.";

        public const string KorisnickoImeRegex = @"^[a-zA-Z0-9._]{3,50}$";
        public const string KorisnickoImePoruka = "Korisničko ime mora imati 3-50 znakova i smije sadržavati samo slova, brojeve, tačku i donju crtu.";

        public const string EmailPoruka = "Unesite validnu email adresu u formatu ime@domena.com.";
        public const string ObaveznoPoljePoruka = "Polje '{0}' je obavezno.";
        public const string MaksimalnaDuzinaPoruka = "Polje '{0}' može imati najviše {1} znakova.";
        public const string LozinkePoklapanjePoruka = "Lozinka i potvrda lozinke se ne poklapaju.";
        public const string RasponPoruka = "Polje '{0}' mora biti između {1} i {2}.";
        public const string OdabirPoruka = "Potrebno je odabrati vrijednost za polje '{0}'.";

        public const string PostanskiBrojRegex = @"^\d{5}$";
        public const string PostanskiBrojPoruka = "Poštanski broj mora imati tačno 5 cifara, npr. 88000.";

        public const string KodPromocijeRegex = @"^[A-Z0-9-]{3,50}$";
        public const string KodPromocijePoruka = "Kod promocije mora imati 3-50 znakova i smije sadržavati samo velika slova, brojeve i crticu, npr. LJETO-2026.";
    }
}
