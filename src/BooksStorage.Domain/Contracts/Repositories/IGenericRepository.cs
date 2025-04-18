using BooksStorage.Domain.Helpers;

namespace BooksStorage.Domain.Contracts.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity // Add constraint to ensure TEntity inherits from BaseEntity
    {
        Task<TEntity?> FindAsync(bool isTracking, CancellationToken token, Expression<Func<TEntity, bool>>? predicate = null);
        Task<bool> TryUpdateAsync(TEntity entity, CancellationToken token);
        Task<bool> TryCreateAsync(TEntity entity, CancellationToken token);
        Task<bool> TryDeleteAsync(Guid id, CancellationToken token);
        IQueryable<TEntity> GetAllQueryable(bool isTracking, Expression<Func<TEntity, bool>>? predicate = null);
        Task<PagedResult<TEntity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken token);
    }
}
