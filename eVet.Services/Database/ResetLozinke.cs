namespace eVet.Services.Database;

/// <summary>
/// Jednokratni kod za promjenu zaboravljene lozinke. Čuva se samo hash koda, uz rok isteka i broj pokušaja.
/// </summary>
public partial class ResetLozinke
{
    public int ResetLozinkeId { get; set; }

    public int KorisnikId { get; set; }

    public string KodHash { get; set; } = null!;

    public string KodSalt { get; set; } = null!;

    public DateTime DatumKreiranja { get; set; }

    public DateTime DatumIsteka { get; set; }

    public int BrojPokusaja { get; set; }

    public bool JeIskoristen { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;
}
