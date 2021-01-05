// <copyright file="QueryableExtensions.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Dtos;

    /// <summary>
    /// Queriable extensions.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Order by query.
        /// </summary>
        /// <typeparam name="T">T.</typeparam>
        /// <param name="source">source.</param>
        /// <param name="sortParameters">sortParameters.</param>
        /// <returns>IQueryable.</returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, SortParametersDto sortParameters)
        {
            var expression = source.Expression;
            int count = 0;
            var parameter = Expression.Parameter(typeof(T), "x");
            var selector = Expression.PropertyOrField(parameter, sortParameters.SortBy);
            var method = string.Equals(sortParameters.OrderBy, "desc", StringComparison.OrdinalIgnoreCase) ?
                (count == 0 ? "OrderByDescending" : "ThenByDescending") :
                (count == 0 ? "OrderBy" : "ThenBy");
            expression = Expression.Call(
                typeof(Queryable),
                method,
                new[] { source.ElementType, selector.Type },
                expression,
                Expression.Quote(Expression.Lambda(selector, parameter)));
            return source.Provider.CreateQuery<T>(expression);
        }
    }
}
