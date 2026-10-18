using eVet.Model;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IKorisniciService : ICRUDService<Korisnici, KorisniciSearchObject, KorisniciInsertRequest, KorisniciUpdateRequest>
    {
        Task<PagedResult<KorisnikOsnovno>> GetPoUloziAsync(string uloga, KorisnikOsnovnoSearchObject search, CancellationToken cancellationToken = default);

        Task<Korisnici> RegistracijaAsync(RegistracijaRequest request, CancellationToken cancellationToken = default);

        Task<Korisnici> GetProfilAsync(CancellationToken cancellationToken = default);

        Task<Korisnici> UpdateProfilAsync(ProfilUpdateRequest request, CancellationToken cancellationToken = default);

        Task PromijeniLozinkuAsync(PromjenaLozinkeRequest request, CancellationToken cancellationToken = default);
    }
}
