namespace BooksStorage.Domain.Contracts.Repositories
{
    public interface IBookSupplierRepository
    {
        Task CreateAsync(Guid bookId, Guid supplierId, CancellationToken token);
        Task<List<BookSupplier>> GetBookSuppliersAsync(CancellationToken token);
        Task<BookSupplier?> GetBookSupplierByIdsAsync(Guid bookId, Guid supplierId, CancellationToken token);
        Task DeleteAsync(Guid bookId, Guid supplierId, CancellationToken token);
    }
}
