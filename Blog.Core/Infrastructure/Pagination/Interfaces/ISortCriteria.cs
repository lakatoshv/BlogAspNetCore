namespace Blog.Core.Infrastructure.Pagination.Interfaces
{
    using Blog.Core.Enums;
    using System.Linq;
    public interface ISortCriteria<T>
    {
        OrderType Direction { get; set; }

        IOrderedQueryable<T> ApplyOrdering(IQueryable<T> query, bool useThenBy);
    }
}
