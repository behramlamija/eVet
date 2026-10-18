namespace eVet.Services.Database;

public partial class NacinPlacanja : ISoftDelete
{
    public int NacinPlacanjaId { get; set; }

    public string Naziv { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual ICollection<Termin> Termini { get; set; } = new List<Termin>();
}
