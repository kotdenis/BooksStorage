namespace BooksStorage.Domain.Entities
{
    public class Publisher : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? ContactInfo { get; private set; }
        public List<Book> Books { get; private set; } = new List<Book>();

        private Publisher() { } // For EF Core

        public Publisher(string name, string? contactInfo)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            Name = name;
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
