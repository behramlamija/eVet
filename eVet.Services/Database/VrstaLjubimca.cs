namespace eVet.Services.Database;

public partial class VrstaLjubimca : ISoftDelete
{
    public int VrstaLjubimcaId { get; set; }

    public string Naziv { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual ICollection<Rasa> Rase { get; set; } = new List<Rasa>();
}
