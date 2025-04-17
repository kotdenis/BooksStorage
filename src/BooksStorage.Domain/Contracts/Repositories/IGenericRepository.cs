namespace BooksStorage.Domain.Contracts.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool isTracking, CancellationToken token);
        Task<bool> TryUpdateAsync(TEntity entity, CancellationToken token);
        Task<bool> TryCreateAsync(TEntity entity, CancellationToken token);
        Task<bool> TryDeleteAsync(Guid id, CancellationToken token);
        Task<IQueryable<TEntity>> GetAllQueryableAsync(Expression<Func<TEntity, bool>> predicate, bool isTracking, CancellationToken token);
    }
}
