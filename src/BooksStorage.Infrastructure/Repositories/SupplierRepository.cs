namespace BooksStorage.Infrastructure.Repositories
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(BooksStorageDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
