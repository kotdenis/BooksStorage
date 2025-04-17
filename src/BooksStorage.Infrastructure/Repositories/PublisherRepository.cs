namespace BooksStorage.Infrastructure.Repositories
{
    public class PublisherRepository : GenericRepository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(BooksStorageDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
