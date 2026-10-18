using eVet.Model.Enums;

namespace eVet.Services.Database;

public partial class Ljubimac : ISoftDelete, ISlikaEntity
{
    public int LjubimacId { get; set; }

    public int KorisnikId { get; set; }

    public string Ime { get; set; } = null!;

    public int RasaId { get; set; }

    public DateOnly? DatumRodjenja { get; set; }

    public Spol Spol { get; set; }

    public decimal? Tezina { get; set; }

    public string? Napomena { get; set; }

    public byte[]? Slika { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual Rasa Rasa { get; set; } = null!;

    public virtual ICollection<Terapija> Terapije { get; set; } = new List<Terapija>();

    public virtual ICollection<Termin> Termini { get; set; } = new List<Termin>();
}
