using System;
using System.Collections.Generic;

namespace eVet.Services.Database;

public partial class RecenzijaOdgovor
{
    public int RecenzijaOdgovorId { get; set; }

    public int RecenzijaId { get; set; }

    public int KorisnikId { get; set; }

    public string Komentar { get; set; } = null!;

    public int? BrojLajkova { get; set; }

    public int? BrojDislajkova { get; set; }

    public DateTime DatumDodavanja { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual Recenzija Recenzija { get; set; } = null!;
}
