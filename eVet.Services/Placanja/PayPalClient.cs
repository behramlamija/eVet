using eVet.Model.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace eVet.Services.Placanja
{
    public class PayPalClient : IPayPalClient
    {
        private const string GreskaZaKorisnika = "Komunikacija s PayPalom trenutno nije moguća. Pokušajte ponovo za nekoliko minuta.";

        // Access token vrijedi nekoliko sati, pa se dijeli između zahtjeva.
        private static readonly SemaphoreSlim TokenLock = new(1, 1);
        private static string? _token;
        private static DateTime _tokenIstice;

        private readonly HttpClient _http;
        private readonly PayPalSettings _settings;
        private readonly ILogger<PayPalClient> _logger;

        public PayPalClient(HttpClient http, IOptions<PayPalSettings> settings, ILogger<PayPalClient> logger)
        {
            _http = http;
            _settings = settings.Value;
            _logger = logger;
            _http.BaseAddress ??= new Uri(_settings.BaseUrl.TrimEnd('/') + "/");
        }

        public async Task<PayPalOrder> KreirajOrderAsync(string referenca, string opis, decimal iznos, string valuta, CancellationToken cancellationToken)
        {
            var tijelo = new JsonObject
            {
                ["intent"] = "CAPTURE",
                ["purchase_units"] = new JsonArray(new JsonObject
                {
                    ["reference_id"] = referenca,
                    ["description"] = opis,
                    ["amount"] = Iznos(iznos, valuta)
                }),
                ["payment_source"] = new JsonObject
                {
                    ["paypal"] = new JsonObject
                    {
                        ["experience_context"] = new JsonObject
                        {
                            ["brand_name"] = "PetCity",
                            ["user_action"] = "PAY_NOW",
                            ["shipping_preference"] = "NO_SHIPPING",
                            ["return_url"] = _settings.ReturnUrl,
                            ["cancel_url"] = _settings.CancelUrl
                        }
                    }
                }
            };

            var odgovor = await PosaljiAsync(HttpMethod.Post, "v2/checkout/orders", tijelo, $"order-{referenca}-{Guid.NewGuid():N}", cancellationToken);
            return ProcitajOrder(odgovor!);
        }

        public async Task<PayPalOrder> GetOrderAsync(string orderId, CancellationToken cancellationToken)
        {
            var odgovor = await PosaljiAsync(HttpMethod.Get, $"v2/checkout/orders/{Uri.EscapeDataString(orderId)}", null, null, cancellationToken);
            return ProcitajOrder(odgovor!);
        }

        public async Task<PayPalOrder> CaptureAsync(string orderId, string idempotencyKey, CancellationToken cancellationToken)
        {
            try
            {
                var odgovor = await PosaljiAsync(HttpMethod.Post, $"v2/checkout/orders/{Uri.EscapeDataString(orderId)}/capture", new JsonObject(), idempotencyKey, cancellationToken);
                return ProcitajOrder(odgovor!);
            }
            catch (PayPalOdbijenoException ex) when (ex.Razlog == "ORDER_ALREADY_CAPTURED")
            {
                // Naplata je već izvršena (npr. ponovljen zahtjev); stanje se čita direktno iz PayPala.
                return await GetOrderAsync(orderId, cancellationToken);
            }
            catch (PayPalOdbijenoException ex) when (ex.Razlog is "ORDER_NOT_APPROVED" or "PAYER_ACTION_REQUIRED")
            {
                throw new BusinessException("Plaćanje još nije odobreno u PayPalu. Završite plaćanje u PayPal prozoru.");
            }
            catch (PayPalOdbijenoException ex) when (ex.Razlog is "INSTRUMENT_DECLINED" or "TRANSACTION_REFUSED")
            {
                throw new BusinessException("PayPal je odbio plaćanje. Pokušajte ponovo s drugim izvorom plaćanja u PayPalu.");
            }
            catch (PayPalOdbijenoException ex)
            {
                throw new BusinessException($"PayPal je odbio naplatu ({ex.Razlog ?? "nepoznat razlog"}). Novac nije naplaćen; pokušajte ponovo ili odaberite plaćanje u stanici.");
            }
        }

        public async Task<PayPalRefund> RefundAsync(string captureId, decimal iznos, string valuta, string idempotencyKey, CancellationToken cancellationToken)
        {
            var tijelo = new JsonObject { ["amount"] = Iznos(iznos, valuta) };
            var odgovor = await PosaljiAsync(HttpMethod.Post, $"v2/payments/captures/{Uri.EscapeDataString(captureId)}/refund", tijelo, idempotencyKey, cancellationToken);

            var amount = odgovor!["amount"];
            return new PayPalRefund(
                (string)odgovor["id"]!,
                (string)odgovor["status"]!,
                amount != null ? ParsirajIznos(amount["value"]) : iznos,
                (string?)amount?["currency_code"] ?? valuta);
        }

        private static JsonObject Iznos(decimal iznos, string valuta) => new()
        {
            ["currency_code"] = valuta,
            ["value"] = iznos.ToString("0.00", CultureInfo.InvariantCulture)
        };

        private static decimal ParsirajIznos(JsonNode? value) => decimal.Parse((string)value!, CultureInfo.InvariantCulture);

        private static PayPalOrder ProcitajOrder(JsonNode order)
        {
            var linkovi = order["links"]?.AsArray() ?? new JsonArray();
            var approve = linkovi.FirstOrDefault(l => (string?)l?["rel"] is "payer-action" or "approve");

            var captureNode = order["purchase_units"]?[0]?["payments"]?["captures"]?[0];
            PayPalCapture? capture = captureNode == null
                ? null
                : new PayPalCapture(
                    (string)captureNode["id"]!,
                    (string)captureNode["status"]!,
                    ParsirajIznos(captureNode["amount"]?["value"]),
                    (string)captureNode["amount"]!["currency_code"]!);

            return new PayPalOrder((string)order["id"]!, (string)order["status"]!, (string?)approve?["href"], capture);
        }

        private async Task<JsonNode?> PosaljiAsync(HttpMethod metoda, string putanja, JsonNode? tijelo, string? idempotencyKey, CancellationToken cancellationToken)
        {
            using var zahtjev = new HttpRequestMessage(metoda, putanja);
            zahtjev.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync(cancellationToken));
            if (idempotencyKey != null)
            {
                zahtjev.Headers.Add("PayPal-Request-Id", idempotencyKey);
            }
            if (tijelo != null)
            {
                zahtjev.Content = new StringContent(tijelo.ToJsonString(), Encoding.UTF8, "application/json");
            }

            using var odgovor = await PosaljiZahtjevAsync(zahtjev, cancellationToken);
            var sadrzaj = await odgovor.Content.ReadAsStringAsync(cancellationToken);
            if (odgovor.IsSuccessStatusCode)
            {
                return string.IsNullOrWhiteSpace(sadrzaj) ? null : JsonNode.Parse(sadrzaj);
            }

            _logger.LogError("PayPal {Metoda} {Putanja} vratio je {Status}: {Odgovor}", metoda, putanja, (int)odgovor.StatusCode, sadrzaj);

            if (odgovor.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                throw new PayPalOdbijenoException((string?)TryParse(sadrzaj)?["details"]?[0]?["issue"]);
            }

            throw new BusinessException(GreskaZaKorisnika);
        }

        private async Task<HttpResponseMessage> PosaljiZahtjevAsync(HttpRequestMessage zahtjev, CancellationToken cancellationToken)
        {
            try
            {
                return await _http.SendAsync(zahtjev, cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "PayPal {Metoda} {Putanja} nije dostupan", zahtjev.Method, zahtjev.RequestUri);
                throw new BusinessException(GreskaZaKorisnika);
            }
        }

        private static JsonNode? TryParse(string sadrzaj)
        {
            try
            {
                return JsonNode.Parse(sadrzaj);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private async Task<string> GetTokenAsync(CancellationToken cancellationToken)
        {
            if (_token != null && DateTime.UtcNow < _tokenIstice)
            {
                return _token;
            }

            await TokenLock.WaitAsync(cancellationToken);
            try
            {
                if (_token != null && DateTime.UtcNow < _tokenIstice)
                {
                    return _token;
                }

                using var zahtjev = new HttpRequestMessage(HttpMethod.Post, "v1/oauth2/token")
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string> { ["grant_type"] = "client_credentials" })
                };
                var kredencijali = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_settings.ClientId}:{_settings.ClientSecret}"));
                zahtjev.Headers.Authorization = new AuthenticationHeaderValue("Basic", kredencijali);

                using var odgovor = await PosaljiZahtjevAsync(zahtjev, cancellationToken);
                var sadrzaj = await odgovor.Content.ReadAsStringAsync(cancellationToken);
                if (!odgovor.IsSuccessStatusCode)
                {
                    _logger.LogError("PayPal autentifikacija nije uspjela ({Status}): {Odgovor}", (int)odgovor.StatusCode, sadrzaj);
                    throw new BusinessException(GreskaZaKorisnika);
                }

                var json = JsonNode.Parse(sadrzaj)!;
                _token = (string)json["access_token"]!;
                // Token se obnavlja minutu prije isteka.
                _tokenIstice = DateTime.UtcNow.AddSeconds((int)json["expires_in"]! - 60);
                return _token;
            }
            finally
            {
                TokenLock.Release();
            }
        }

        private sealed class PayPalOdbijenoException : BusinessException
        {
            public PayPalOdbijenoException(string? razlog) : base(GreskaZaKorisnika)
            {
                Razlog = razlog;
            }

            public string? Razlog { get; }
        }
    }
}
