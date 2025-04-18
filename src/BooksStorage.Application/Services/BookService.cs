namespace BooksStorage.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookService> _logger;
        private readonly IValidator<BookDto> _bookValidator;
        private readonly IValidator<UpdateBookDto> _updBookValidator;

        public BookService(IUnitOfWork unitOfWork,
            ILogger<BookService> logger,
            IValidator<BookDto> bookValidator,
            IValidator<UpdateBookDto> updBookValidator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bookValidator = bookValidator ?? throw new ArgumentNullException(nameof(bookValidator));
            _updBookValidator = updBookValidator ?? throw new ArgumentNullException(nameof(updBookValidator));
        }

        public async IAsyncEnumerable<BookDto> GetBooksAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var books = await _unitOfWork.BookRepository.GetAllQueryable(isTracking: false)
                .Include(b => b.Publisher)
                .Include(b => b.Suppliers)
                .ToListAsync(cancellationToken);
            foreach (var book in books)
            {
                yield return new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Price = book.Price,
                    Description = book.Description,
                    PublisherId = book.PublisherId,
                    SupplierNames = book.Suppliers.Select(s => s.SupplierName).ToList(),
                };
            }
        }

        public async Task<PagedResultDto<BookDto>> GetPagedBooksAsync(int pageNumber = 1, int pageSize = 10, CancellationToken token = default)
        {
            var pagedResult = await _unitOfWork.BookRepository.GetPagedAsync(pageNumber, pageSize, token);
            var bookDtos = pagedResult.Items.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Price = b.Price,
                PublisherId = b.PublisherId,
                PublisherName = b.Publisher!.PublisherName,
                SupplierNames = b.Suppliers.Select(s => s.SupplierName).ToList()
            });

            return new PagedResultDto<BookDto>
            {
                Items = bookDtos,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount
            };
        }

        public async Task<BookDto> GetBookByIdAsync(Guid id, CancellationToken token)
        {
            var book = await _unitOfWork.BookRepository.GetAllQueryable(isTracking: false, b => b.Id == id)
                .Include(b => b.Publisher)
                .Include(b => b.Suppliers)
                .FirstOrDefaultAsync(token)
                ?? throw new ArgumentException($"Книга по {id} не найдена.");
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Price = book.Price,
                Description = book.Description,
                PublisherId = book.PublisherId,
                SupplierNames = book.Suppliers.Select(s => s.SupplierName).ToList(),
            };
        }

        public async Task UpdateBookAsync(Guid id, UpdateBookDto dto, CancellationToken token)
        {
            await _updBookValidator.ValidateAndThrowAsync(dto);
            
            await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, token);
            try
            {
                var book = await _unitOfWork.BookRepository.GetAllQueryable(isTracking: true, b => b.Id == id)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(token)
                ?? throw new ArgumentException($"Книга по {id} не найдена.");
                if (dto.Price.HasValue)
                {
                    book.UpdatePrice(dto.Price.Value);
                }
                book.AddDetails(dto.Title!, dto.Author!, dto.Description);

                if (dto.PublisherId.HasValue)
                {
                    var publisher = await _unitOfWork.PublisherRepository.FindAsync(isTracking: true, token, b => b.Id == dto.PublisherId)
                        ?? throw new ArgumentException($"Издатель по {dto.PublisherId} не найден.");
                    book.AssignPublisher(publisher);
                }

                await _unitOfWork.BookRepository.TryUpdateAsync(book, token);
                await _unitOfWork.TrySaveChangesAsync(token);
                await _unitOfWork.CommitTransactionAsync(token);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Ошибка в обновлении книги.");
                throw;
            }
        }

        public async Task DeleteBookAsync(Guid id, CancellationToken token)
        {
            var book = await _unitOfWork.BookRepository.GetAllQueryable(isTracking: true, b => b.Id == id)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(token)
                ?? throw new ArgumentException($"Книга по {id} не найдена.");
            await _unitOfWork.BookRepository.TryDeleteAsync(book.Id, token);
            await _unitOfWork.TrySaveChangesAsync(token);
        }

        public async Task CreateBookAsync(BookDto dto, CancellationToken token)
        {
            await _bookValidator.ValidateAndThrowAsync(dto);
            await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, token);
            try
            {
                var book = new Book(dto.Title, dto.Author, dto.Price, dto.Description, dto.PublisherId);
                var publisher = await _unitOfWork.PublisherRepository.FindAsync(isTracking: true, token, b => b.Id == dto.PublisherId)
                    ?? throw new ArgumentException($"Издатель по {dto.PublisherId} не найден.");
                book.AssignPublisher(publisher);
                await _unitOfWork.BookRepository.TryCreateAsync(book, token);
                await _unitOfWork.TrySaveChangesAsync(token);
                await _unitOfWork.CommitTransactionAsync(token);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(token);
                _logger.LogError(ex, "Ошибка в создании книги.");
                throw;
            }
        }
    }
}
