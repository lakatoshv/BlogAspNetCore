namespace AuthService.Core.Infrastructure
{
    using Enums;
    using AuthService.Core.Infrastructure.Pagination.Interfaces;
    using System;
    using System.Linq;

    /// <summary>
    /// Field sort order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ISortCriteria{T}" />
    public class FieldSortOrder<T> : ISortCriteria<T> where T : class
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public OrderType Direction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldSortOrder{T}"/> class.
        /// </summary>
        public FieldSortOrder()
        {
            Direction = OrderType.Ascending;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldSortOrder{T}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="direction">The direction.</param>
        public FieldSortOrder(string name, OrderType direction)
        {
            Name = name;
            Direction = direction;
        }

        /// <summary>
        /// Applies the ordering.
        /// </summary>
        /// <param name="qry">The qry.</param>
        /// <param name="useThenBy">if set to <c>true</c> [use then by].</param>
        /// <returns>IOrderedQueryable.</returns>
        public IOrderedQueryable<T> ApplyOrdering(IQueryable<T> qry, Boolean useThenBy)
        {
            IOrderedQueryable<T> result;
            var descending = Direction == OrderType.Descending;
            result = !useThenBy ? qry.OrderBy(Name, descending) : qry.ThenBy(Name, descending);
            return result;
        }
    }
}
