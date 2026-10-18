namespace eVet.Services.Database;

public partial class Korisnik : ISoftDelete, ISlikaEntity
{
    public int KorisnikId { get; set; }

    public string Ime { get; set; } = null!;

    public string Prezime { get; set; } = null!;

    public string KorisnickoIme { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefon { get; set; }

    public byte[]? Slika { get; set; }

    public string LozinkaHash { get; set; } = null!;

    public string LozinkaSalt { get; set; } = null!;

    public bool JeAktivan { get; set; } = true;

    public string SecurityStamp { get; set; } = null!;

    public DateTime DatumRegistracije { get; set; }

    // Podaci o zaposlenju popunjavaju se samo za veterinare.
    public DateOnly? DatumRodjenja { get; set; }

    public int? GradRodjenjaId { get; set; }

    public DateOnly? DatumZaposlenja { get; set; }

    public string? Zvanje { get; set; }

    public int? SpecijalnostId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual Grad? GradRodjenja { get; set; }

    public virtual Specijalnost? Specijalnost { get; set; }

    public virtual ICollection<AktiviranaPromocija> AktiviranePromocije { get; set; } = new List<AktiviranaPromocija>();

    public virtual ICollection<KorisnikUloga> KorisniciUloge { get; set; } = new List<KorisnikUloga>();

    public virtual ICollection<Ljubimac> Ljubimci { get; set; } = new List<Ljubimac>();

    public virtual ICollection<Obavijest> Obavijesti { get; set; } = new List<Obavijest>();

    public virtual ICollection<Obaveza> Obaveze { get; set; } = new List<Obaveza>();

    public virtual ICollection<RecenzijaOdgovor> RecenzijaOdgovori { get; set; } = new List<RecenzijaOdgovor>();

    public virtual ICollection<Recenzija> Recenzije { get; set; } = new List<Recenzija>();

    public virtual ICollection<Terapija> Terapije { get; set; } = new List<Terapija>();

    public virtual ICollection<Termin> TerminiKlijenta { get; set; } = new List<Termin>();

    public virtual ICollection<Termin> TerminiVeterinara { get; set; } = new List<Termin>();
}
