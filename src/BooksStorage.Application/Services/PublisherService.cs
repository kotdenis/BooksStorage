namespace BooksStorage.Application.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PublisherService> _logger;
        private readonly IValidator<PublisherDto> _validator;

        public PublisherService(IUnitOfWork unitOfWork, 
            ILogger<PublisherService> logger, 
            IValidator<PublisherDto> validator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<IEnumerable<PublisherDto>> GetAllPublishersAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var publishers = await _unitOfWork.PublisherRepository.GetAllQueryable(isTracking: false)
                .Include(p => p.Books)
                .ToListAsync(); 
            return publishers.Select(p => new PublisherDto
            {
                Id = p.Id,
                Name = p.PublisherName,
                ContactInfo = p.ContactInfo,
                BookTitles = p.Books.Select(b => b.Title).ToList()
            });
        }

        public async Task<PublisherDto?> GetPublisherByIdAsync(Guid id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var publisher = await _unitOfWork.PublisherRepository.GetAllQueryable(isTracking: false, p => p.Id == id)
                .Include(p => p.Books)
                .FirstOrDefaultAsync();
            if (publisher == null) return null;

            return new PublisherDto
            {
                Id = publisher.Id,
                Name = publisher.PublisherName,
                ContactInfo = publisher.ContactInfo,
                BookTitles = publisher.Books.Select(b => b.Title).ToList()
            };
        }

        public async Task CreatePublisherAsync(PublisherDto dto, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _validator.ValidateAndThrowAsync(dto, token);
            var publisher = new Publisher(dto.Name, dto.ContactInfo);
            await _unitOfWork.PublisherRepository.TryCreateAsync(publisher, token);
            await _unitOfWork.TrySaveChangesAsync(token);
        }

        public async Task UpdatePublisherAsync(Guid id, PublisherDto dto, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _validator.ValidateAndThrowAsync(dto, token);
            var publisher = await _unitOfWork.PublisherRepository.FindAsync(true, token, p => p.Id == id)   
                ?? throw new ArgumentException($"Издатель по {id} не найден.");

            publisher.UpdateDetails(dto.Name, dto.ContactInfo);

            await _unitOfWork.PublisherRepository.TryUpdateAsync(publisher, token);
            await _unitOfWork.TrySaveChangesAsync(token);
        }

        public async Task DeletePublisherAsync(Guid id, CancellationToken token)
        {
            var publisher = await _unitOfWork.PublisherRepository.GetAllQueryable(isTracking: false, p => p.Id == id)
                .Include(p => p.Books)
                .FirstOrDefaultAsync(token)
                ?? throw new ArgumentException($"Издатель по {id} не найден.");
            if (publisher.Books.Any())
                throw new InvalidOperationException($"Нельзя удалить издателя книги которого присутствуют.");
            await _unitOfWork.PublisherRepository.TryDeleteAsync(id, token);
            await _unitOfWork.TrySaveChangesAsync(token);
        }
    }
}
