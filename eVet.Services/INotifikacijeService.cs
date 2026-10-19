using eVet.Services.Database;

namespace eVet.Services
{
    public interface INotifikacijeService
    {
        /// <summary>
        /// Dodaje obavijest u isti DbContext kao i poslovnu promjenu (snima se zajedno s njom)
        /// i priprema e-mail koji se šalje tek nakon uspješnog snimanja.
        /// </summary>
        void Dodaj(Korisnik primalac, string naslov, string sadrzaj);

        /// <summary>Šalje pripremljene e-mailove na RabbitMQ; poziva se nakon SaveChangesAsync.</summary>
        Task PosaljiEmailoveAsync(CancellationToken cancellationToken = default);
    }
}
