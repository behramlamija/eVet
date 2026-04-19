using eVet.Model;
using eVet.Services.Database;
using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services
{
    public interface IKorisniciService
    {
        PagedResult<Korisnici> GetAll(KorisniciSearchObject searchObject);
        Korisnici Insert (KorisniciInsertRequest request);
        Korisnici Update (int id, KorisniciUpdateRequest request);

    }
}
