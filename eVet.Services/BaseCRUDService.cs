using eVet.Model.Exceptions;
using eVet.Model.SearchObjects;
using eVet.Services.Database;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eVet.Services
{
    public abstract class BaseCRUDService<TModel, TSearch, TDbEntity, TInsert, TUpdate> : BaseService<TModel, TSearch, TDbEntity>, ICRUDService<TModel, TSearch, TInsert, TUpdate> where TModel : class where TSearch : BaseSearchObject where TDbEntity : class
    {
        protected BaseCRUDService(EVetDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public virtual async Task<TModel> InsertAsync(TInsert request, CancellationToken cancellationToken = default)
        {
            TDbEntity entity = Mapper.Map<TDbEntity>(request!);
            await BeforeInsertAsync(request, entity, cancellationToken);

            Context.Add(entity);
            await Context.SaveChangesAsync(cancellationToken);

            return Mapper.Map<TModel>(entity);
        }

        public virtual async Task<TModel> UpdateAsync(int id, TUpdate request, CancellationToken cancellationToken = default)
        {
            var entity = await GetEntityForUpdateAsync(id, cancellationToken);

            Mapper.Map(request, entity);
            await BeforeUpdateAsync(request, entity, cancellationToken);

            await Context.SaveChangesAsync(cancellationToken);

            return Mapper.Map<TModel>(entity);
        }

        public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await GetEntityForUpdateAsync(id, cancellationToken);

            await BeforeDeleteAsync(entity, cancellationToken);

            if (entity is ISoftDelete softDelete)
            {
                softDelete.IsDeleted = true;
                softDelete.VrijemeBrisanja = DateTime.UtcNow;
            }
            else
            {
                Context.Remove(entity);
            }

            await Context.SaveChangesAsync(cancellationToken);
        }

        protected virtual async Task<TDbEntity> GetEntityForUpdateAsync(int id, CancellationToken cancellationToken)
        {
            return await AddInclude(ApplyPristup(Context.Set<TDbEntity>()))
                .FirstOrDefaultAsync(e => EF.Property<int>(e, KeyName) == id, cancellationToken)
                ?? throw new NotFoundException($"{NazivEntiteta} sa traženim identifikatorom ne postoji.");
        }

        protected virtual IQueryable<TDbEntity> AddInclude(IQueryable<TDbEntity> query)
        {
            return query;
        }

        protected async Task ProvjeriDaNePostojiAsync(Expression<Func<TDbEntity, bool>> uslov, string poruka, CancellationToken cancellationToken)
        {
            if (await Context.Set<TDbEntity>().AnyAsync(uslov, cancellationToken))
            {
                throw new BusinessException(poruka);
            }
        }

        protected virtual Task BeforeInsertAsync(TInsert request, TDbEntity entity, CancellationToken cancellationToken) => Task.CompletedTask;

        protected virtual Task BeforeUpdateAsync(TUpdate request, TDbEntity entity, CancellationToken cancellationToken) => Task.CompletedTask;

        // Ovdje servisi provjeravaju da li zapis koriste drugi entiteti i bacaju BusinessException sa razlogom.
        protected virtual Task BeforeDeleteAsync(TDbEntity entity, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
