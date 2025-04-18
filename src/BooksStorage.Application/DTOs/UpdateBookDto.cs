namespace BooksStorage.Application.DTOs
{
    public record UpdateBookDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public Guid? PublisherId { get; set; }
    }
}
