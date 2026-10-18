namespace eVet.Services.Database;

public partial class Specijalnost : ISoftDelete
{
    public int SpecijalnostId { get; set; }

    public string Naziv { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual ICollection<Korisnik> Veterinari { get; set; } = new List<Korisnik>();
}
