namespace BooksStorage.Application.Services
{
    public class PublisherService
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


    }
}
