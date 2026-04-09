using System;
using System.Collections.Generic;

namespace eVet.Services.Database;

public partial class Ljubimac
{
    public int LjubimacId { get; set; }

    public int KorisnikId { get; set; }

    public string Ime { get; set; } = null!;

    public string Vrsta { get; set; } = null!;

    public string? Rasa { get; set; }

    public DateOnly? DatumRodjenja { get; set; }

    public string? Spol { get; set; }

    public decimal? Tezina { get; set; }

    public string? Napomena { get; set; }

    public byte[]? Slika { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual ICollection<Terapija> Terapijas { get; set; } = new List<Terapija>();

    public virtual ICollection<Termin> Termins { get; set; } = new List<Termin>();
}
