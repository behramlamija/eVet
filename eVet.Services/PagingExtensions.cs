using eVet.Model;
using eVet.Model.SearchObjects;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    public static class PagingExtensions
    {
        // Query mora biti sortiran prije poziva; projekcija u DTO izvršava se u SQL-u.
        public static async Task<PagedResult<TModel>> ToPagedResultAsync<TEntity, TModel>(
            this IQueryable<TEntity> query,
            IQueryable<TEntity> countQuery,
            BaseSearchObject search,
            TypeAdapterConfig config,
            CancellationToken cancellationToken)
        {
            int count = await countQuery.CountAsync(cancellationToken);

            int page = Math.Max(search.Page ?? 0, 0);
            int pageSize = Math.Clamp(search.PageSize ?? BaseSearchObject.DefaultPageSize, 1, BaseSearchObject.MaxPageSize);

            var list = await query
                .Skip(page * pageSize)
                .Take(pageSize)
                .ProjectToType<TModel>(config)
                .ToListAsync(cancellationToken);

            return new PagedResult<TModel> { Count = count, ResultList = list };
        }
    }
}
