namespace BooksStorage.Application.Contracts
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherDto>> GetAllPublishersAsync(CancellationToken token);
        Task<PublisherDto?> GetPublisherByIdAsync(Guid id, CancellationToken token);
        Task CreatePublisherAsync(PublisherDto dto, CancellationToken token);
        Task UpdatePublisherAsync(Guid id, PublisherDto dto, CancellationToken token);
        Task DeletePublisherAsync(Guid id, CancellationToken token);
    }
}
