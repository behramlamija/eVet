using eVet.Model.Enums;

namespace eVet.Services.Database;

public partial class Termin : ISoftDelete
{
    public int TerminId { get; set; }

    public int KorisnikId { get; set; }

    public int VeterinarId { get; set; }

    public int LjubimacId { get; set; }

    public string Sifra { get; set; } = null!;

    public DateOnly DatumTermina { get; set; }

    public TimeOnly VrijemePocetka { get; set; }

    public TimeOnly VrijemeKraja { get; set; }

    public int UkupnoTrajanje { get; set; }

    public decimal IznosPopusta { get; set; }

    public decimal CijenaPrevoza { get; set; }

    public decimal UkupnaCijena { get; set; }

    public StatusTermina Status { get; set; } = StatusTermina.NaCekanju;

    public int NacinPlacanjaId { get; set; }

    public int? AktiviranaPromocijaId { get; set; }

    public bool JePrevoz { get; set; }

    public string? AdresaPrevoza { get; set; }

    public int? GradPrevozaId { get; set; }

    public DateTime DatumKreiranja { get; set; }

    public DateTime? DatumPromjeneStatusa { get; set; }

    public int? StatusPromijenioId { get; set; }

    public string? RazlogPromjeneStatusa { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual AktiviranaPromocija? AktiviranaPromocija { get; set; }

    public virtual Grad? GradPrevoza { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual Ljubimac Ljubimac { get; set; } = null!;

    public virtual NacinPlacanja NacinPlacanja { get; set; } = null!;

    public virtual Korisnik? StatusPromijenio { get; set; }

    public virtual Korisnik Veterinar { get; set; } = null!;

    public virtual Recenzija? Recenzija { get; set; }

    public virtual ICollection<StavkaTermina> StavkeTermina { get; set; } = new List<StavkaTermina>();

    public virtual ICollection<Terapija> Terapije { get; set; } = new List<Terapija>();

    public virtual ICollection<Uplata> Uplate { get; set; } = new List<Uplata>();
}
