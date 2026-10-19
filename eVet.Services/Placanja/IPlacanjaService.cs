using eVet.Model;
using eVet.Services.Database;

namespace eVet.Services.Placanja
{
    public interface IPlacanjaService
    {
        /// <summary>Otvara PayPal narudžbu za termin (ili vraća već otvorenu). Iznos određuje server.</summary>
        Task<PayPalNarudzba> ZapocniPayPalAsync(int terminId, CancellationToken cancellationToken = default);

        /// <summary>Server-side naplata i potvrda uplate. Idempotentno: potvrđena uplata se ne obrađuje ponovo.</summary>
        Task<Model.Termini> PotvrdiPayPalAsync(int uplataId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Pri otkazivanju ili odbijanju termina vraća stvarno naplaćeni iznos i zatvara otvorene uplate.
        /// Promjene se snimaju zajedno s promjenom statusa termina.
        /// </summary>
        Task ObradiOtkazivanjeAsync(Termin termin, CancellationToken cancellationToken = default);
    }
}
