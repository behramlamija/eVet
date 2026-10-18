namespace eVet.Services.Database;

public partial class Rasa : ISoftDelete
{
    public int RasaId { get; set; }

    public string Naziv { get; set; } = null!;

    public int VrstaLjubimcaId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual VrstaLjubimca VrstaLjubimca { get; set; } = null!;

    public virtual ICollection<Ljubimac> Ljubimci { get; set; } = new List<Ljubimac>();
}
