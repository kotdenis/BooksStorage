namespace BooksStorage.Application.Contracts
{
    public interface ISupplierService
    {
        IAsyncEnumerable<SupplierDto> GetSuppliersAsync(CancellationToken cancellationToken = default);
        Task<SupplierDto> GetSupplierByIdAsync(Guid id, CancellationToken token);
        Task CreateSupplierAsync(SupplierDto dto, CancellationToken token);
        Task<PagedResultDto<SupplierDto>> GetPagedSuppliersAsync(int pageNumber = 1, int pageSize = 10, CancellationToken token = default);
        Task UpdateSupplierAsync(Guid id, SupplierDto dto, CancellationToken token);
        Task DeleteSupplierAsync(Guid id, CancellationToken token);
    }
}
