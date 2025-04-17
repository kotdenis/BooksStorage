namespace BooksStorage.Infrastructure.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(BooksStorageDbContext dbContext) : base(dbContext)
        {
        }
    }
}
