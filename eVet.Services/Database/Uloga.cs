namespace eVet.Services.Database;

public partial class Uloga : ISoftDelete
{
    public int UlogaId { get; set; }

    public string Naziv { get; set; } = null!;

    public string? Opis { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual ICollection<KorisnikUloga> KorisniciUloge { get; set; } = new List<KorisnikUloga>();
}
