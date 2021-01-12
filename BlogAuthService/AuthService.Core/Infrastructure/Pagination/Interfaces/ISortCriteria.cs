namespace AuthService.Core.Infrastructure.Pagination.Interfaces
{
    using AuthService.Core.Enums;
    using System.Linq;

    /// <summary>
    /// Sort criteria interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISortCriteria<T>
    {
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        OrderType Direction { get; set; }

        /// <summary>
        /// Applies the ordering.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="useThenBy">if set to <c>true</c> [use then by].</param>
        /// <returns>IOrderedQueryable.</returns>
        IOrderedQueryable<T> ApplyOrdering(IQueryable<T> query, bool useThenBy);
    }
}
