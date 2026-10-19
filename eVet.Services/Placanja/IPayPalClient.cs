namespace eVet.Services.Placanja
{
    public record PayPalOrder(string Id, string Status, string? ApproveUrl, PayPalCapture? Capture);

    public record PayPalCapture(string Id, string Status, decimal Iznos, string Valuta);

    public record PayPalRefund(string Id, string Status, decimal Iznos, string Valuta);

    /// <summary>PayPal REST API (Orders v2 i Payments v2).</summary>
    public interface IPayPalClient
    {
        Task<PayPalOrder> KreirajOrderAsync(string referenca, string opis, decimal iznos, string valuta, CancellationToken cancellationToken);

        Task<PayPalOrder> GetOrderAsync(string orderId, CancellationToken cancellationToken);

        /// <summary>Naplata odobrenog ordera. Isti <paramref name="idempotencyKey"/> nikad ne naplaćuje dvaput.</summary>
        Task<PayPalOrder> CaptureAsync(string orderId, string idempotencyKey, CancellationToken cancellationToken);

        /// <summary>Povrat novca za naplatu. Isti <paramref name="idempotencyKey"/> nikad ne vraća novac dvaput.</summary>
        Task<PayPalRefund> RefundAsync(string captureId, decimal iznos, string valuta, string idempotencyKey, CancellationToken cancellationToken);
    }
}
