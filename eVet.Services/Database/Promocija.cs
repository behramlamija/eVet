namespace eVet.Services.Database;

public partial class Promocija : ISoftDelete
{
    public int PromocijaId { get; set; }

    public string Naziv { get; set; } = null!;

    public string? Opis { get; set; }

    public string Kod { get; set; } = null!;

    public decimal Popust { get; set; }

    public DateTime DatumPocetka { get; set; }

    public DateTime DatumKraja { get; set; }

    public int UslugaId { get; set; }

    public bool JeAktivna { get; set; } = true;

    public bool IsDeleted { get; set; }

    public DateTime? VrijemeBrisanja { get; set; }

    public virtual ICollection<AktiviranaPromocija> AktiviranePromocije { get; set; } = new List<AktiviranaPromocija>();

    public virtual Usluga Usluga { get; set; } = null!;
}
