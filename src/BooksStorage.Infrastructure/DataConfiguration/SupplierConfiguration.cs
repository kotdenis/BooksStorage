namespace BooksStorage.Infrastructure.DataConfiguration
{
    public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.SupplierName).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Location).HasMaxLength(200);
        }
    }
}
