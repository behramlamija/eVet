using eVet.Messaging;
using eVet.Services.Database;
using Microsoft.Extensions.Logging;

namespace eVet.Services
{
    public class NotifikacijeService : INotifikacijeService
    {
        private readonly EVetDbContext _context;
        private readonly IPorukePublisher _publisher;
        private readonly ILogger<NotifikacijeService> _logger;
        private readonly List<EmailPoruka> _naCekanju = new();

        public NotifikacijeService(EVetDbContext context, IPorukePublisher publisher, ILogger<NotifikacijeService> logger)
        {
            _context = context;
            _publisher = publisher;
            _logger = logger;
        }

        public void Dodaj(Korisnik primalac, string naslov, string sadrzaj)
        {
            _context.Obavijesti.Add(new Obavijest
            {
                Korisnik = primalac,
                Naslov = naslov,
                Sadrzaj = sadrzaj,
                DatumObavijesti = DateTime.UtcNow
            });

            _naCekanju.Add(new EmailPoruka(primalac.Email, $"{primalac.Ime} {primalac.Prezime}", naslov, sadrzaj));
        }

        public async Task PosaljiEmailoveAsync(CancellationToken cancellationToken = default)
        {
            var poruke = _naCekanju.ToList();
            _naCekanju.Clear();

            foreach (var poruka in poruke)
            {
                try
                {
                    await _publisher.PosaljiEmailAsync(poruka, cancellationToken);
                }
                catch (Exception ex)
                {
                    // Poslovna promjena i obavijest u aplikaciji su već sačuvane; neuspjeh slanja e-maila se samo bilježi.
                    _logger.LogError(ex, "E-mail \"{Naslov}\" za {Primalac} nije poslan na RabbitMQ", poruka.Naslov, poruka.Primalac);
                }
            }
        }
    }
}
