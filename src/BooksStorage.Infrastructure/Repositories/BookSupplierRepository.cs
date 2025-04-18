namespace BooksStorage.Infrastructure.Repositories
{
    public class BookSupplierRepository : IBookSupplierRepository
    {
        private readonly BooksStorageDbContext _dbContext;
        public BookSupplierRepository(BooksStorageDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task CreateAsync(Guid bookId, Guid supplierId, CancellationToken token)
        {
            _dbContext.Set<BookSupplier>().Add(new BookSupplier(bookId, supplierId));
            return Task.CompletedTask;
        }
        public Task<List<BookSupplier>> GetBookSuppliersAsync(CancellationToken token)
        {
            return _dbContext.Set<BookSupplier>().ToListAsync(token);
        }
        public Task<BookSupplier?> GetBookSupplierByIdsAsync(Guid bookId, Guid supplierId, CancellationToken token)
        {
            var entity = _dbContext.Set<BookSupplier>()
                .FirstOrDefaultAsync(x => x.BookId == bookId && x.SupplierId == supplierId, token);
            return entity;
        }

        public async Task DeleteAsync(Guid bookId, Guid supplierId, CancellationToken token)
        {
            var entity = await _dbContext.Set<BookSupplier>()
                .FirstOrDefaultAsync(x => x.BookId == bookId && x.SupplierId == supplierId, token);
            if (entity != null)
            {
                _dbContext.Set<BookSupplier>().Remove(entity);
            }
        }
    }
}
