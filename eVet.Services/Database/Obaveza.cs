namespace eVet.Services.Database;

public partial class Obaveza : ISoftDelete
{
    public int ObavezaId { get; set; }

    public int VeterinarId { get; set; }

    public string Naslov { get; set; } = null!;

    public string? Opis { get; set; }

    public DateTime DatumVrijeme { get; set; }

    public bool JeZavrsena { get; set; }

    public DateTime DatumKreiranja { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual Korisnik Veterinar { get; set; } = null!;
}
