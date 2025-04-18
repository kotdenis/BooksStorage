namespace BooksStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks(CancellationToken token = default)
        {
            var books = await _bookService.GetBooksAsync(token).ToListAsync(token);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(Guid id, CancellationToken token = default)
        {
            var book = await _bookService.GetBookByIdAsync(id, token);
            if (book == null)
                return NotFound();
            return Ok(book);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResultDto<BookDto>>> GetPagedBooks([FromQuery] int pageNumber,[FromQuery] int pageSize, CancellationToken token = default)
        {
            var pagedBooks = await _bookService.GetPagedBooksAsync(pageNumber, pageSize, token);
            return Ok(pagedBooks);
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] BookDto dto, CancellationToken token = default)
        {
            await _bookService.CreateBookAsync(dto, token);
            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] UpdateBookDto dto, CancellationToken token = default)
        {
            try
            {
                await _bookService.UpdateBookAsync(id, dto, token);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id, CancellationToken token = default)
        {
            try
            {
                await _bookService.DeleteBookAsync(id, token);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
