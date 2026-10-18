namespace eVet.Services.Database;

public partial class KorisnikUloga
{
    public int KorisnikUlogaId { get; set; }

    public int KorisnikId { get; set; }

    public int UlogaId { get; set; }

    public DateTime DatumDodavanja { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual Uloga Uloga { get; set; } = null!;
}
