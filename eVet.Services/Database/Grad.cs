namespace eVet.Services.Database;

public partial class Grad : ISoftDelete
{
    public int GradId { get; set; }

    public string Naziv { get; set; } = null!;

    public string PostanskiBroj { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual ICollection<Korisnik> Korisnici { get; set; } = new List<Korisnik>();

    public virtual ICollection<Termin> Termini { get; set; } = new List<Termin>();
}
