using BooksStorage.Domain.Helpers;

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


        public async Task<TEntity?> FindAsync(bool isTracking, CancellationToken token, Expression<Func<TEntity, bool>>? predicate = null)
        {
            token.ThrowIfCancellationRequested();
            if (predicate == null)
                predicate = e => true;
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

        public IQueryable<TEntity> GetAllQueryable(bool isTracking, Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
                predicate = e => true;
            IQueryable<TEntity> query = _entities;
            if (isTracking == false)
                query = _dbContext.Set<TEntity>()
                    .Where(predicate)
                    .AsNoTracking();
            else
                query = _dbContext.Set<TEntity>()
                    .Where(predicate);
            return query;
        }

        public async Task<PagedResult<TEntity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "pageNumber должен быть больше 0.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), "pageSize должен быть больше 0.");
            var query = ApplyIncludes();
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResult<TEntity>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
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

        private IQueryable<TEntity> ApplyIncludes()
        {
            var query = _entities.AsQueryable();
            if (typeof(TEntity) == typeof(Book))
                query = query.Include("Publisher").Include("Suppliers");
            else if (typeof(TEntity) == typeof(Supplier))
                query = query.Include("Books");
            else if (typeof(TEntity) == typeof(Publisher))
                query = query.Include("Books");
            return query;
        }
    }
}
