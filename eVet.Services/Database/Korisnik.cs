using System;
using System.Collections.Generic;

namespace eVet.Services.Database;

public partial class Korisnik
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

    public bool? JeAktivan { get; set; }

    public DateTime DatumRegistracije { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual ICollection<AktiviranaPromocija> AktiviranaPromocijas { get; set; } = new List<AktiviranaPromocija>();

    public virtual ICollection<KorisniciUloge> KorisniciUloges { get; set; } = new List<KorisniciUloge>();

    public virtual ICollection<Ljubimac> Ljubimacs { get; set; } = new List<Ljubimac>();

    public virtual ICollection<Obavijest> Obavijests { get; set; } = new List<Obavijest>();

    public virtual ICollection<RecenzijaOdgovor> RecenzijaOdgovors { get; set; } = new List<RecenzijaOdgovor>();

    public virtual ICollection<Recenzija> Recenzijas { get; set; } = new List<Recenzija>();

    public virtual ICollection<Terapija> Terapijas { get; set; } = new List<Terapija>();

    public virtual ICollection<Termin> TerminKorisniks { get; set; } = new List<Termin>();

    public virtual ICollection<Termin> TerminVeterinars { get; set; } = new List<Termin>();
}
