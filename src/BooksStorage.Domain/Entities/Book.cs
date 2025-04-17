namespace BooksStorage.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; private set; } = string.Empty;
        public string Author { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string? Description { get; set; }
        public Guid PublisherId { get; private set; }
        public Publisher? Publisher { get; private set; }
        public List<Supplier> Suppliers { get; private set; } = new();

        private Book() { } // For EF Core

        public Book(string title, string author, decimal price, string? description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Должно быть наименование книги.", nameof(title));
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Должно быть наименование автора.", nameof(author));
            if (price <= 0)
                throw new ArgumentException("Цена должна быть больше нуля.", nameof(price));
            Title = title;
            Author = author;
            Price = price;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentException("Цена должна быть больше нуля.", nameof(newPrice));
            Price = newPrice;
        }

        public void AssignPublisher(Publisher publisher)
        {
            if (publisher == null)
                throw new ArgumentNullException(nameof(publisher));
            Publisher = publisher;
            PublisherId = publisher.Id;
        }

        public void AddSupplier(Supplier supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier));
            if (!Suppliers.Contains(supplier))
                Suppliers.Add(supplier);
        }

        public void RemoveSupplier(Supplier supplier)
        {
            if (supplier == null)
                throw new ArgumentNullException(nameof(supplier));
            Suppliers.Remove(supplier);
        }
    }
}
