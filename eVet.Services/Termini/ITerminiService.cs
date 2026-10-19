using eVet.Model.Requests;
using eVet.Model.SearchObjects;

namespace eVet.Services.Termini
{
    public interface ITerminiService : IService<Model.Termini, TerminiSearchObject>
    {
        Task<Model.Termini> KreirajAsync(TerminInsertRequest request, CancellationToken cancellationToken = default);

        Task<Model.Termini> PotvrdiAsync(int id, CancellationToken cancellationToken = default);

        Task<Model.Termini> OdbijAsync(int id, PromjenaStatusaRequest request, CancellationToken cancellationToken = default);

        Task<Model.Termini> OtkaziAsync(int id, PromjenaStatusaRequest request, CancellationToken cancellationToken = default);

        Task<Model.Termini> ZavrsiAsync(int id, CancellationToken cancellationToken = default);

        Model.TerminiKonfiguracija GetKonfiguracija();

        Task<List<TimeOnly>> GetSlobodnaVremenaAsync(int veterinarId, DateOnly datum, int trajanje, CancellationToken cancellationToken = default);
    }
}
