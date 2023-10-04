using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.SharedKernel.Interfaces
{
    public interface IBaseRepository<TEntity, TKey> : IBaseReadonlyRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
	{
        void Add(TEntity entity);
     
        void AddRange(IList<TEntity> entities);
        
        void Update(TEntity entity);
        
        void UpdateRange(IList<TEntity> entities);
        
        void Remove(TEntity entity);
        
        void RemoveRange(IList<TEntity> entities);

        Task CommitAsync(CancellationToken cancellationToken = default);

	}
}
