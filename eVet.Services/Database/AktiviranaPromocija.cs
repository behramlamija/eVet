namespace eVet.Services.Database;

public partial class AktiviranaPromocija : ISoftDelete
{
    public int AktiviranaPromocijaId { get; set; }

    public int PromocijaId { get; set; }

    public int KorisnikId { get; set; }

    public bool Iskoristena { get; set; }

    public DateTime DatumAktiviranja { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual Promocija Promocija { get; set; } = null!;

    public virtual ICollection<Termin> Termini { get; set; } = new List<Termin>();
}
