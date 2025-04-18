namespace BooksStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService ?? throw new ArgumentNullException(nameof(supplierService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSuppliers(CancellationToken token = default)
        {
            var suppliers = await _supplierService.GetSuppliersAsync(token).ToListAsync(token);
            return Ok(suppliers);
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResultDto<SupplierDto>>> GetPagedBooks([FromQuery] int pageNumber, [FromQuery] int pageSize, CancellationToken token = default)
        {
            var pagedBooks = await _supplierService.GetPagedSuppliersAsync(pageNumber, pageSize, token);
            return Ok(pagedBooks);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDto>> GetSupplier(Guid id, CancellationToken token = default)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id, token);
            if (supplier == null)
                return NotFound();
            return Ok(supplier);
        }

        [HttpPost]
        public async Task<ActionResult<SupplierDto>> CreateSupplier([FromBody] SupplierDto dto, CancellationToken token = default)
        {
            await _supplierService.CreateSupplierAsync(dto, token);
            return Ok(dto); 
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(Guid id,[FromBody] SupplierDto dto, CancellationToken token = default)
        {
            await _supplierService.UpdateSupplierAsync(id, dto, token);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(Guid id, CancellationToken token = default)
        {
            await _supplierService.DeleteSupplierAsync(id, token);
            return NoContent();
        }
    }
}
