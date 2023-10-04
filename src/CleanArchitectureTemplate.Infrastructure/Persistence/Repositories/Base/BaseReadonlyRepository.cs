using Microsoft.EntityFrameworkCore;
using CleanArchitectureTemplate.SharedKernel.Interfaces;
using CleanArchitectureTemplate.SharedKernel.Types;
using System.Linq.Expressions;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories.Base
{
    public class BaseReadonlyRepository<TEntity, TKey> : IBaseReadonlyRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected readonly EFDbContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        public BaseReadonlyRepository(EFDbContext context)
        {
            dbContext = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual ValueTask<TEntity> FindAsync(TKey id)
        {
            return dbSet.FindAsync(id);
        }

        public virtual Task<int> GetCountAsync()
        {
            return dbSet.CountAsync();
        }

        public virtual Task<List<TEntity>> GetListAsync()
        {
            return dbSet.ToListAsync();
        }

        public virtual Task<long> GetSumAsync(Expression<Func<TEntity, long>> sumExpression)
        {
            return dbSet.SumAsync(sumExpression);
        }

        #region Protected Methods

        protected async Task<PagedResult<TEntity>> GetPagedList(IQueryable<TEntity> query, int pageNumber, int pageSize)
        {
            var total = await query.CountAsync();
            var results = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<TEntity>(results, total, pageNumber, pageSize);
        } 
        #endregion
    }
}
