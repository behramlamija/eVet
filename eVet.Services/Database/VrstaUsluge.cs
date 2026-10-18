namespace eVet.Services.Database;

public partial class VrstaUsluge : ISoftDelete, ISlikaEntity
{
    public int VrstaId { get; set; }

    public string Naziv { get; set; } = null!;

    public bool JeOperacija { get; set; }

    public bool JeVakcinacija { get; set; }

    public byte[]? Slika { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual ICollection<Usluga> Usluge { get; set; } = new List<Usluga>();
}
