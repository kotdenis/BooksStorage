namespace BooksStorage.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BooksStorageDbContext _dbContext;
        private readonly Lazy<IBookRepository> _bookRepository;
        private readonly Lazy<IPublisherRepository> _publisherRepository;
        private readonly Lazy<ISupplierRepository> _supplierRepository;
        private readonly Lazy<IBookSupplierRepository> _bookSupplierRepository;
        private readonly ILogger<UnitOfWork> _logger;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(BooksStorageDbContext dbContext, ILogger<UnitOfWork> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _bookRepository = new Lazy<IBookRepository>(() => new BookRepository(dbContext));
            _publisherRepository = new Lazy<IPublisherRepository>(() => new PublisherRepository(dbContext));
            _supplierRepository = new Lazy<ISupplierRepository>(() => new SupplierRepository(dbContext));
            _bookSupplierRepository = new Lazy<IBookSupplierRepository>(() => new BookSupplierRepository(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IBookRepository BookRepository => _bookRepository.Value;
        public IPublisherRepository PublisherRepository => _publisherRepository.Value;
        public ISupplierRepository SupplierRepository => _supplierRepository.Value;
        public IBookSupplierRepository BookSupplierRepository => _bookSupplierRepository.Value;


        public async Task BeginTransactionAsync(System.Data.IsolationLevel isolationLevel, CancellationToken token = default)
        {
            if (_currentTransaction != null)
                return;
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel);
        }

        public async Task CommitTransactionAsync(CancellationToken token = default)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("Транзакция не началась.");
            try
            {
                await _currentTransaction.CommitAsync(token);
            }
            catch
            {
                await RollbackTransactionAsync(token);
                _logger.LogError("Ошибка в выполнении транзакции.");
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public Task RollbackTransactionAsync(CancellationToken token = default)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("Транзакция не началась.");
            try
            {
                _currentTransaction.Rollback();
            }
            finally
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
            return Task.CompletedTask;
        }

        public async Task<bool> TrySaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении изменений в базе данных.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении базы данных.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Неизвестная ошибка при сохранении изменений в базе данных.");
            }
            return false;
        }
    }
}
