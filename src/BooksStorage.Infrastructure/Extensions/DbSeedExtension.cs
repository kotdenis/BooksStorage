namespace BooksStorage.Infrastructure.Extensions
{
    public static class DbSeedExtension
    {
        public static async Task PopulateDatabaseAsync(this IApplicationBuilder app, IConfiguration configuration, IHostEnvironment environment)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<BooksStorageDbContext>();

            try
            {
                dbContext.Database.Migrate();
                await PopulatePublishersAsync(dbContext);
                await PopulateBooksAsync(dbContext);
                await PopulateSuppliersAsync(dbContext);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                
            }
        }

        public static async Task PopulateBooksAsync(BooksStorageDbContext dbContext)
        {
            if (!dbContext.Set<Book>().Any())
            {
                var publishers = await dbContext.Set<Publisher>().ToListAsync();
                var publisher1 = publishers.FirstOrDefault(p => p.PublisherName == "Publisher1");
                var publisher2 = publishers.FirstOrDefault(p => p.PublisherName == "Publisher2");
                var books = new Book[]
                {
                    new Book("1984", "George Orwell", 100, "", publisher1!.Id),
                    new Book("It", "Stephen King", 150, "", publisher1!.Id),
                    new Book("Salem's lot", "Stephen King", 200, "", publisher2!.Id)
                };
                dbContext.Set<Book>().AddRange(books);
                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task PopulateSuppliersAsync(BooksStorageDbContext dbContext)
        {
            if (!dbContext.Set<Supplier>().Any())
            {
                var suppliers = new Supplier[]
                {
                    new Supplier("Supplier1", "Address1"),
                    new Supplier("Supplier2", "Address2")
                };
                dbContext.Set<Supplier>().AddRange(suppliers);
                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task PopulatePublishersAsync(BooksStorageDbContext dbContext)
        {
            if (!dbContext.Set<Publisher>().Any())
            {
                var publishers = new Publisher[]
                {
                    new Publisher("Publisher1", ""),
                    new Publisher("Publisher2", "")
                };
                dbContext.Set<Publisher>().AddRange(publishers);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
