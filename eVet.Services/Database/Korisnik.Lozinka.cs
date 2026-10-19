using eVet.Services.Security;

namespace eVet.Services.Database;

public partial class Korisnik
{
    /// <summary>Postavlja novu lozinku (PBKDF2 sa novim saltom).</summary>
    public void PostaviLozinku(string lozinka)
    {
        LozinkaSalt = PasswordHasher.GenerateSalt();
        LozinkaHash = PasswordHasher.Hash(lozinka, LozinkaSalt);
        SecurityStamp ??= Guid.NewGuid().ToString();
    }

    /// <summary>Poništava sve ranije izdane tokene korisnika (odjava sa svih uređaja).</summary>
    public void PonistiPrijave() => SecurityStamp = Guid.NewGuid().ToString();

    public bool ProvjeriLozinku(string lozinka) => PasswordHasher.Verify(lozinka, LozinkaSalt, LozinkaHash);
}
