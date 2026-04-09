using System;
using System.Collections.Generic;

namespace eVet.Services.Database;

public partial class Terapija
{
    public int TerapijaId { get; set; }

    public int LjubimacId { get; set; }

    public int VeterinarId { get; set; }

    public int? TerminId { get; set; }

    public string Dijagnoza { get; set; } = null!;

    public string? Lijek { get; set; }

    public string? Doza { get; set; }

    public string? Napomena { get; set; }

    public DateTime DatumTerapije { get; set; }

    public DateOnly? DatumKontrolnog { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual Ljubimac Ljubimac { get; set; } = null!;

    public virtual Termin? Termin { get; set; }

    public virtual Korisnik Veterinar { get; set; } = null!;
}
