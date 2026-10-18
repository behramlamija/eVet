namespace eVet.Services.Database;

public partial class RecenzijaOdgovor : ISoftDelete
{
    public int RecenzijaOdgovorId { get; set; }

    public int RecenzijaId { get; set; }

    public int KorisnikId { get; set; }

    public string Komentar { get; set; } = null!;

    public DateTime DatumDodavanja { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual Recenzija Recenzija { get; set; } = null!;
}
