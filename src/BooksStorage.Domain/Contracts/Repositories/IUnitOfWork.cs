namespace BooksStorage.Domain.Contracts.Repositories
{
    public interface IUnitOfWork
    {
        IBookRepository BookRepository { get; }
        IPublisherRepository PublisherRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        IBookSupplierRepository BookSupplierRepository { get; }
        Task BeginTransactionAsync(System.Data.IsolationLevel isolationLevel, CancellationToken token = default);
        Task CommitTransactionAsync(CancellationToken token = default);
        Task RollbackTransactionAsync(CancellationToken token = default);
        Task<bool> TrySaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
