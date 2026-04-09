using System;
using System.Collections.Generic;

namespace eVet.Services.Database;

public partial class Termin
{
    public int TerminId { get; set; }

    public int KorisnikId { get; set; }

    public int VeterinarId { get; set; }

    public int LjubimacId { get; set; }

    public string Sifra { get; set; } = null!;

    public DateOnly DatumTermina { get; set; }

    public TimeOnly VrijemePocetka { get; set; }

    public TimeOnly VrijemeKraja { get; set; }

    public decimal UkupnaCijena { get; set; }

    public int UkupnoTrajanje { get; set; }

    public int? UkupanBrojUsluga { get; set; }

    public string? StateMachine { get; set; }

    public int NacinPlacanjaId { get; set; }

    public int? AktiviranaPromocijaId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual AktiviranaPromocija? AktiviranaPromocija { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual Ljubimac Ljubimac { get; set; } = null!;

    public virtual NacinPlacanja NacinPlacanja { get; set; } = null!;

    public virtual ICollection<StavkeTermina> StavkeTerminas { get; set; } = new List<StavkeTermina>();

    public virtual ICollection<Terapija> Terapijas { get; set; } = new List<Terapija>();

    public virtual Korisnik Veterinar { get; set; } = null!;
}
