namespace BooksStorage.Application.DTOs
{
    public record SupplierDto
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public List<string> BookTitles { get; set; } = new List<string>();
        public List<Guid> BookIds { get; set; } = new List<Guid>();
    }
}
