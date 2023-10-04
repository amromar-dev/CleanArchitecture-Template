using CleanArchitectureTemplate.SharedKernel.Types;
using System.Linq.Expressions;

namespace CleanArchitectureTemplate.SharedKernel.Interfaces
{
    public interface IBaseReadonlyRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
	{
        Task<List<TEntity>> GetListAsync();
        
        ValueTask<TEntity> FindAsync(TKey id);
        
        Task<int> GetCountAsync();
        
        Task<long> GetSumAsync(Expression<Func<TEntity, long>> sumExpression);
    }
}
