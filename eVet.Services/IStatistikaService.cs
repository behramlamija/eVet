using eVet.Model;
using eVet.Model.Enums;
using eVet.Model.SearchObjects;
using eVet.Model.Statistika;

namespace eVet.Services
{
    public interface IStatistikaService
    {
        Task<StatistikaPoslovanja> GetPoslovanjeAsync(DateOnly? datumOd, DateOnly? datumDo, CancellationToken cancellationToken = default);

        Task<List<StatistikaPeriod>> GetPoPeriodimaAsync(GrupisanjeStatistike grupisanje, int? godina, CancellationToken cancellationToken = default);

        Task<PagedResult<StatistikaVeterinara>> GetVeterinareAsync(KorisnikOsnovnoSearchObject search, CancellationToken cancellationToken = default);
    }
}
