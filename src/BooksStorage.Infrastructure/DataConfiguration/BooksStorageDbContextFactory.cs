namespace BooksStorage.Infrastructure.DataConfiguration
{
    public class BooksStorageDbContextFactory : IDesignTimeDbContextFactory<BooksStorageDbContext>
    {
        public BooksStorageDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BooksStorageDbContext>();

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../BooksStorage.Api"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var connectionString = configuration.GetConnectionString("NpgConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Нет строки подключения.");
            }
            optionsBuilder.UseNpgsql(connectionString);
            return new BooksStorageDbContext(optionsBuilder.Options);
        }
    }
}
