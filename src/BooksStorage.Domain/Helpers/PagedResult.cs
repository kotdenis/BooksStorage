namespace BooksStorage.Domain.Helpers
{
    public class PagedResult<TEntity> where TEntity : BaseEntity
    {
        public IEnumerable<TEntity> Items { get; set; } = new List<TEntity>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
