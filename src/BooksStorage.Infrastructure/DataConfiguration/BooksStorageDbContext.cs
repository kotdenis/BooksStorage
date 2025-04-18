namespace BooksStorage.Infrastructure.DataConfiguration
{
    public class BooksStorageDbContext : DbContext
    {
        public BooksStorageDbContext(DbContextOptions<BooksStorageDbContext> options)
            : base(options)
        {
        }
        public DbSet<Book>? Books { get; set; } = null;
        public DbSet<Publisher>? Publishers { get; set; } = null;
        public DbSet<Supplier>? Suppliers { get; set; } = null;
        public DbSet<BookSupplier>? BookSuppliers { get; set; } = null;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new PublisherConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.OnBeforeSaving();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            this.OnBeforeSaving();
            return base.SaveChanges();
        }
    }
}
