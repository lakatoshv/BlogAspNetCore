namespace Blog.Core.Infrastructure.Pagination
{
    using System.Collections.Generic;
    public class PagedListResult<TEntity>
    {
        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }

        public int Count { get; set; }

        public IEnumerable<TEntity> Entities { get; set; }
    }
}
