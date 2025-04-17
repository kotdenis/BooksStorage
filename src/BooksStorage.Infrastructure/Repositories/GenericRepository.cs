namespace BooksStorage.Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly BooksStorageDbContext _dbContext;
        private DbSet<TEntity> _entities;

        public GenericRepository(BooksStorageDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _entities = dbContext.Set<TEntity>();
        }


        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, bool isTracking, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            TEntity? result = null;
            if (isTracking == false)
                result = await _dbContext.Set<TEntity>()
                .Where(predicate)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            else
                result = await _dbContext.Set<TEntity>()
                    .Where(predicate)
                    .FirstOrDefaultAsync();
            if (result == null)
                throw new InvalidOperationException($"Не найдена необходимая сущность.");
            return result;
        }

        public Task<IQueryable<TEntity>> GetAllQueryableAsync(Expression<Func<TEntity, bool>> predicate, bool isTracking, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            IQueryable<TEntity> query = _entities;
            if (isTracking == false)
                query = _dbContext.Set<TEntity>()
                    .Where(predicate)
                    .AsNoTracking();
            else
                query = _dbContext.Set<TEntity>()
                    .Where(predicate);
            return Task.FromResult(query);
        }

        public Task<bool> TryCreateAsync(TEntity entity, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _entities.Add(entity);
            return Task.FromResult(true);
        }

        public async Task<bool> TryDeleteAsync(Guid id, CancellationToken token)
        {
            var entity = await _entities.FindAsync(id, token);
            if (entity == null)
                throw new InvalidOperationException($"Не найдена сущность с идентификатором {id}.");
            _entities.Remove(entity);
            return true;
        }

        public Task<bool> TryUpdateAsync(TEntity entity, CancellationToken token)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(Update));
            _dbContext.Update(entity);
            return Task.FromResult(true);
        }
    }
}
