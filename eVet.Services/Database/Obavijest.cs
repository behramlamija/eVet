using System;
using System.Collections.Generic;

namespace eVet.Services.Database;

public partial class Obavijest
{
    public int ObavijestId { get; set; }

    public int KorisnikId { get; set; }

    public string Naslov { get; set; } = null!;

    public string Sadrzaj { get; set; } = null!;

    public bool JePogledana { get; set; }

    public DateTime DatumObavijesti { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual Korisnik Korisnik { get; set; } = null!;
}
