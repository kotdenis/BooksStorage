namespace BooksStorage.Application.DTOs
{
    public record PublisherDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ContactInfo { get; set; }
        public List<string> BookTitles { get; set; } = new List<string>();
    }
}
