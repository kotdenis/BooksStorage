namespace BooksStorage.Application.Contracts
{
    public interface IBookService
    {
        IAsyncEnumerable<BookDto> GetBooksAsync(CancellationToken cancellationToken = default);
        Task<BookDto> GetBookByIdAsync(Guid id, CancellationToken token);
        Task<PagedResultDto<BookDto>> GetPagedBooksAsync(int pageNumber = 1, int pageSize = 10, CancellationToken token = default);
        Task CreateBookAsync(BookDto bookDto, CancellationToken token);
        Task UpdateBookAsync(Guid id, UpdateBookDto bookDto, CancellationToken token);
        Task DeleteBookAsync(Guid id, CancellationToken token);
    }
}
