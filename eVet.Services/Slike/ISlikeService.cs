using eVet.Model.Enums;

namespace eVet.Services.Slike
{
    public interface ISlikeService
    {
        Task<SlikaDatoteka> GetAsync(TipSlike tip, int id, CancellationToken cancellationToken = default);

        Task SetAsync(TipSlike tip, int id, byte[] sadrzaj, string? contentType, CancellationToken cancellationToken = default);

        Task DeleteAsync(TipSlike tip, int id, CancellationToken cancellationToken = default);
    }
}
