namespace BooksStorage.Domain.Entities
{
    public class Publisher : BaseEntity
    {
        public string PublisherName { get; private set; } = string.Empty;
        public string? ContactInfo { get; private set; }
        public List<Book> Books { get; private set; } = new List<Book>();

        private Publisher() { } // For EF Core

        public Publisher(string name, string? contactInfo)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя издателя не должно быть пустым.", nameof(name));

            PublisherName = name;
            ContactInfo = contactInfo;
        }

        public void UpdateDetails(string name, string? contactInfo)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя издателя не должно быть пустым.", nameof(name));
            PublisherName = name;
            ContactInfo = contactInfo;
        }

        public void AddBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));
            if (!Books.Contains(book))
                Books.Add(book);
        }

        public void RemoveBook(Book book)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));
            Books.Remove(book);
        }
    }
}
