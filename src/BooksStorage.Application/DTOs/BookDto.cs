namespace BooksStorage.Application.DTOs
{
    public record BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string PublisherName { get; set; } = string.Empty;
        public Guid PublisherId { get; set; }
        public List<string> SupplierNames { get; set; } = new();
    }
}
