using System;
using System.Collections.Generic;

namespace eVet.Services.Database;

public partial class Usluga
{
    public int UslugaId { get; set; }

    public string Naziv { get; set; } = null!;

    public string? Opis { get; set; }

    public decimal Cijena { get; set; }

    public int Trajanje { get; set; }

    public byte[]? Slika { get; set; }

    public DateTime DatumObjavljivanja { get; set; }

    public int VrstaId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual ICollection<Promocija> Promocijas { get; set; } = new List<Promocija>();

    public virtual ICollection<Recenzija> Recenzijas { get; set; } = new List<Recenzija>();

    public virtual ICollection<StavkeTermina> StavkeTerminas { get; set; } = new List<StavkeTermina>();

    public virtual VrstaUsluge Vrsta { get; set; } = null!;
}
