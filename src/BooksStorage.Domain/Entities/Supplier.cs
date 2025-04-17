namespace BooksStorage.Domain.Entities
{
    public class Supplier : BaseEntity
    {
        public string SupplierName { get; private set; } = string.Empty;
        public string? Location { get; private set; }
        public List<Book> Books { get; private set; } = new ();

        private Supplier() { } // For EF Core

        public Supplier(string supplierName, string? location)
        {
            if (string.IsNullOrWhiteSpace(supplierName))
                throw new ArgumentException("Должно быть наименование поставщика.", nameof(supplierName));
            SupplierName = supplierName;
            Location = location;
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
