namespace Blog.Core.Infrastructure
{
    using Blog.Core.Enums;
    using Blog.Core.Infrastructure.Pagination.Interfaces;
    using System;
    using System.Linq;
    public class FieldSortOrder<T> : ISortCriteria<T> where T : class
    {
        public String Name { get; set; }

        public OrderType Direction { get; set; }

        public FieldSortOrder()
        {
            Direction = OrderType.Ascending;
        }

        public FieldSortOrder(string name, OrderType direction)
        {
            Name = name;
            Direction = direction;
        }

        public IOrderedQueryable<T> ApplyOrdering(IQueryable<T> qry, Boolean useThenBy)
        {
            IOrderedQueryable<T> result;
            var descending = Direction == OrderType.Descending;
            result = !useThenBy ? qry.OrderBy(Name, descending) : qry.ThenBy(Name, descending);
            return result;
        }
    }
}
