namespace BooksStorage.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SupplierService> _logger;
        private readonly IValidator<SupplierDto> _validator;

        public SupplierService(IUnitOfWork unitOfWork,
            ILogger<SupplierService> logger, 
            IValidator<SupplierDto> validator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async IAsyncEnumerable<SupplierDto> GetSuppliersAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var suppliers = await _unitOfWork.SupplierRepository.GetAllQueryable(isTracking: false)
                .Include(s => s.Books)
                .ToListAsync(cancellationToken);
            foreach (var supplier in suppliers)
            {
                yield return new SupplierDto
                {
                    Id = supplier.Id,
                    SupplierName = supplier.SupplierName,
                    Location = supplier.Location,
                    BookTitles = supplier.Books.Select(b => b.Title).ToList(),
                };
            }
        }

        public async Task<PagedResultDto<SupplierDto>> GetPagedSuppliersAsync(int pageNumber = 1, int pageSize = 10, CancellationToken token = default)
        {
            var pagedResult = await _unitOfWork.SupplierRepository.GetPagedAsync(pageNumber, pageSize, token);
            var supplierDtos = pagedResult.Items.Select(s => new SupplierDto
            {
                Id = s.Id,
                SupplierName = s.SupplierName,
                Location = s.Location,
                BookTitles = s.Books.Select(b => b.Title).ToList(),
            });
            return new PagedResultDto<SupplierDto>
            {
                Items = supplierDtos,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount
            };
        }

        public async Task<SupplierDto> GetSupplierByIdAsync(Guid id, CancellationToken token)
        {
            var supplier = await _unitOfWork.SupplierRepository.GetAllQueryable(isTracking: false, s => s.Id == id)
                .Include(s => s.Books)
                .FirstOrDefaultAsync(token)
                ?? throw new ArgumentException($"Поставщик по {id} не найден.");
            return new SupplierDto
            {
                Id = supplier.Id,
                SupplierName = supplier.SupplierName,
                Location = supplier.Location,
                BookTitles = supplier.Books.Select(b => b.Title).ToList(),
            };
        }

        public async Task CreateSupplierAsync(SupplierDto dto, CancellationToken token)
        {
            await _validator.ValidateAndThrowAsync(dto, token);
            var supplier = new Supplier(dto.SupplierName, dto.Location);
            await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, token);
            try
            {
                foreach (var bookId in dto.BookIds)
                {
                    var book = await _unitOfWork.BookRepository.GetAllQueryable(isTracking: true, b => b.Id == bookId)
                        .Include(b => b.Suppliers)
                        .FirstOrDefaultAsync()
                        ?? throw new ArgumentException($"Книга по {bookId} не найдена.");
                    book.Suppliers.Add(supplier);
                }

                await _unitOfWork.SupplierRepository.TryCreateAsync(supplier, token);
                await _unitOfWork.TrySaveChangesAsync(token);
                await _unitOfWork.CommitTransactionAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _unitOfWork.RollbackTransactionAsync(token);
                throw;
            }
        }

        public async Task UpdateSupplierAsync(Guid id, SupplierDto dto, CancellationToken token)
        {
            await _validator.ValidateAndThrowAsync(dto);
            await _unitOfWork.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
            try
            {
                var supplier = await _unitOfWork.SupplierRepository.GetAllQueryable(isTracking: true, s => s.Id == id)
                .Include(s => s.Books)
                .FirstOrDefaultAsync()
                ?? throw new ArgumentException($"Supplier with ID {id} not found.");

                supplier.UpdateDetails(dto.SupplierName, dto.Location);

                if (dto.BookIds != null)
                {
                    supplier.Books.Clear();
                    foreach (var bookId in dto.BookIds)
                    {
                        var book = await _unitOfWork.BookRepository.GetAllQueryable(isTracking: true, b => b.Id == bookId)
                            .Include(b => b.Suppliers)
                            .FirstOrDefaultAsync()
                            ?? throw new ArgumentException($"Книга по {bookId} не найдена.");
                        book.Suppliers.Add(supplier);
                    }
                }
                await _unitOfWork.SupplierRepository.TryUpdateAsync(supplier, token);
                await _unitOfWork.TrySaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _unitOfWork.RollbackTransactionAsync(token);
                throw;
            }
        }

        public async Task DeleteSupplierAsync(Guid id, CancellationToken token)
        {
            var supplier = await _unitOfWork.SupplierRepository.GetAllQueryable(isTracking: true, s => s.Id == id)
                .Include(s => s.Books)
                .FirstOrDefaultAsync(token)
                ?? throw new ArgumentException($"Supplier with ID {id} not found.");
            await _unitOfWork.SupplierRepository.TryDeleteAsync(id, token);
            await _unitOfWork.TrySaveChangesAsync(token);
        }
    }
}
