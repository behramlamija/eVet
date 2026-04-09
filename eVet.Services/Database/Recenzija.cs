using System;
using System.Collections.Generic;

namespace eVet.Services.Database;

public partial class Recenzija
{
    public int RecenzijaId { get; set; }

    public int KorisnikId { get; set; }

    public int UslugaId { get; set; }

    public string Komentar { get; set; } = null!;

    public int? BrojLajkova { get; set; }

    public int? BrojDislajkova { get; set; }

    public DateTime DatumDodavanja { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;

    public virtual ICollection<RecenzijaOdgovor> RecenzijaOdgovors { get; set; } = new List<RecenzijaOdgovor>();

    public virtual Usluga Usluga { get; set; } = null!;
}
