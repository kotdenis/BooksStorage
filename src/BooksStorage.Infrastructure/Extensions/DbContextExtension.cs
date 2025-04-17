namespace BooksStorage.Infrastructure.Extensions
{
    public static class DbContextExtension
    {
        public static void OnBeforeSaving(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    var now = DateTime.UtcNow;

                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            baseEntity.UpdatedAt = now;
                            break;

                        case EntityState.Added:
                            baseEntity.CreatedAt = now;
                            baseEntity.UpdatedAt = now;
                            break;
                    }
                }
            }
        }
    }
}
