namespace eVet.Services.Database;

public partial class Recenzija : ISoftDelete
{
    public int RecenzijaId { get; set; }

    public int KorisnikId { get; set; }

    public int TerminId { get; set; }

    public bool UslugaPozitivna { get; set; }

    public bool VeterinarPozitivan { get; set; }

    public string? Komentar { get; set; }

    public DateTime DatumDodavanja { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual Termin Termin { get; set; } = null!;

    public virtual ICollection<RecenzijaOdgovor> RecenzijaOdgovori { get; set; } = new List<RecenzijaOdgovor>();
}
