// <copyright file="ISortCriteria.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Core.Infrastructure.Pagination.Interfaces
{
    using System.Linq;
    using Enums;

    /// <summary>
    /// Sort items by criteria interface.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public interface ISortCriteria<T>
    {
        /// <summary>
        /// Gets or sets direction.
        /// </summary>
        OrderType Direction { get; set; }

        /// <summary>
        /// Apply ordering.
        /// </summary>
        /// <param name="query">query.</param>
        /// <param name="useThenBy">useThenBy.</param>
        /// <returns>IOrderedQueryable.</returns>
        IOrderedQueryable<T> ApplyOrdering(IQueryable<T> query, bool useThenBy);
    }
}
