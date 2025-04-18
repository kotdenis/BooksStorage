namespace BooksStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetPublishers(CancellationToken token = default)
        {
            var publishers = await _publisherService.GetAllPublishersAsync(token);
            return Ok(publishers);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDto>> GetPublisher(Guid id, CancellationToken token = default)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id, token);
            if (publisher == null)
                return NotFound();
            return Ok(publisher);
        }
        [HttpPost]
        public async Task<ActionResult<PublisherDto>> CreatePublisher([FromBody] PublisherDto dto, CancellationToken token = default)
        {
            await _publisherService.CreatePublisherAsync(dto, token);
            return Ok(dto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(Guid id, [FromBody] PublisherDto dto, CancellationToken token = default)
        {
            await _publisherService.UpdatePublisherAsync(id, dto, token);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(Guid id, CancellationToken token = default)
        {
            await _publisherService.DeletePublisherAsync(id, token);
            return NoContent();
        }
    }
}
