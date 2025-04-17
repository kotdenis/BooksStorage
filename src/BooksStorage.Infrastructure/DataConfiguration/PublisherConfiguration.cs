namespace BooksStorage.Infrastructure.DataConfiguration
{
    public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.ToTable("Publishers");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PublisherName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.ContactInfo)
                .HasMaxLength(200);
        }
    }
}
