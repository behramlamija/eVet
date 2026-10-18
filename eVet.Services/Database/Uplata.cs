using eVet.Model.Enums;

namespace eVet.Services.Database;

public partial class Uplata
{
    public int UplataId { get; set; }

    public int TerminId { get; set; }

    public decimal Iznos { get; set; }

    public string Valuta { get; set; } = null!;

    public StatusUplate Status { get; set; } = StatusUplate.NaCekanju;

    public string PayPalOrderId { get; set; } = null!;

    public string? PayPalCaptureId { get; set; }

    public DateTime DatumKreiranja { get; set; }

    public DateTime? DatumPlacanja { get; set; }

    public decimal? IznosRefunda { get; set; }

    public string? PayPalRefundId { get; set; }

    public DateTime? DatumRefunda { get; set; }

    public virtual Termin Termin { get; set; } = null!;
}
