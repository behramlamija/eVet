using eVet.Model;
using eVet.Model.Exceptions;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace eVet.Services
{
    public abstract class BaseService<TModel, TSearch, TDbEntity> : IService<TModel, TSearch> where TSearch : BaseSearchObject where TDbEntity : class where TModel : class
    {
        protected EVetDbContext Context { get; }
        protected IMapper Mapper { get; }

        protected BaseService(EVetDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        protected virtual string NazivEntiteta => typeof(TDbEntity).Name;

        public virtual async Task<PagedResult<TModel>> GetPagedAsync(TSearch search, CancellationToken cancellationToken = default)
        {
            var query = AddFilter(search, ApplyPristup(Context.Set<TDbEntity>().AsNoTracking()));

            // Projekcija se izvršava u SQL-u, pa se iz baze čitaju samo kolone potrebne za DTO (bez slika).
            return await ApplyOrdering(query).ToPagedResultAsync<TDbEntity, TModel>(query, search, Mapper.Config, cancellationToken);
        }

        public virtual async Task<TModel> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await ApplyPristup(Context.Set<TDbEntity>().AsNoTracking())
                .Where(e => EF.Property<int>(e, KeyName) == id)
                .ProjectToType<TModel>(Mapper.Config)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException($"{NazivEntiteta} sa traženim identifikatorom ne postoji.");
        }

        protected virtual IQueryable<TDbEntity> AddFilter(TSearch search, IQueryable<TDbEntity> query)
        {
            return query;
        }

        // Ograničava zapise na one kojima trenutni korisnik smije pristupiti (npr. klijent vidi samo svoje).
        protected virtual IQueryable<TDbEntity> ApplyPristup(IQueryable<TDbEntity> query)
        {
            return query;
        }

        // Najnoviji zapisi se prikazuju prvi.
        protected virtual IQueryable<TDbEntity> ApplyOrdering(IQueryable<TDbEntity> query)
        {
            return query.OrderByDescending(e => EF.Property<int>(e, KeyName));
        }

        protected string KeyName => Context.Model.FindEntityType(typeof(TDbEntity))!.FindPrimaryKey()!.Properties[0].Name;
    }
}
