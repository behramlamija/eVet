namespace eVet.Services.Database;

public partial class Usluga : ISoftDelete, ISlikaEntity
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

    public virtual ICollection<Promocija> Promocije { get; set; } = new List<Promocija>();

    public virtual ICollection<StavkaTermina> StavkeTermina { get; set; } = new List<StavkaTermina>();

    public virtual VrstaUsluge Vrsta { get; set; } = null!;
}
