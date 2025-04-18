namespace BooksStorage.Domain.Entities
{
    public class BookSupplier
    {
        public Guid BookId { get;  set; }
        public Book Book { get;  set; } = null!;
        public Guid SupplierId { get;  set; }
        public Supplier Supplier { get;  set; } = null!;
        private BookSupplier() { } // For EF Core

        public BookSupplier(Guid bookId, Guid supplierId)
        {
            BookId = bookId;
            SupplierId = supplierId;
        }
    }
}
