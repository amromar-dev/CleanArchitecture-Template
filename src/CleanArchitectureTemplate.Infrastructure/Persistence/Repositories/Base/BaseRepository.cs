using CleanArchitectureTemplate.SharedKernel.Interfaces;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories.Base
{
    public class BaseRepository<TEntity, TKey> : BaseReadonlyRepository<TEntity, TKey>, IBaseRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected readonly EFDbContext context;

        public BaseRepository(EFDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(IList<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IList<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }

        public void UpdateRange(IList<TEntity> entities)
        {
            dbSet.UpdateRange(entities);
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
