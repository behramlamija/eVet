using System;
using System.Collections.Generic;

namespace eVet.Services.Database;

public partial class StavkeTermina
{
    public int StavkeTerminaId { get; set; }

    public int TerminId { get; set; }

    public int UslugaId { get; set; }

    public decimal? Cijena { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual Termin Termin { get; set; } = null!;

    public virtual Usluga Usluga { get; set; } = null!;
}
