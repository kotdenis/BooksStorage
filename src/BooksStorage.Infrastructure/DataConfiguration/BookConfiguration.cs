namespace BooksStorage.Infrastructure.DataConfiguration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Title)
                .HasMaxLength(200)
                .IsRequired();
            builder.Property(b => b.Author)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(b => b.PublisherId)
                .IsRequired();
            builder.Property(b => b.CreatedAt)
                .IsRequired();
            builder.Property(b => b.UpdatedAt)
                .IsRequired();

            builder.HasOne(b => b.Publisher)
                   .WithMany(p => p.Books)
                   .HasForeignKey(b => b.PublisherId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Suppliers)
                   .WithMany(s => s.Books)
                   .UsingEntity(j => j.ToTable("BookSupplier"));
        }
    }
}
