// <copyright file="QueryableExtensions.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

using System;

namespace Blog.Core.Infrastructure
{
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Queryable extensions.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Order By.
        /// </summary>
        /// <typeparam name="TEntity">TEntity.</typeparam>
        /// <param name="source">source.</param>
        /// <param name="orderByProperty">orderByProperty.</param>
        /// <param name="desc">desc.</param>
        /// <returns>IOrderedQueryable.</returns>
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool desc)
            where TEntity : class
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property ?? throw new InvalidOperationException());
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
            return (IOrderedQueryable<TEntity>)source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        /// <summary>
        /// Then By.
        /// </summary>
        /// <typeparam name="TEntity">TEntity.</typeparam>
        /// <param name="source">source.</param>
        /// <param name="orderByProperty">orderByProperty.</param>
        /// <param name="desc">desc.</param>
        /// <returns>IOrderedQueryable.</returns>
        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool desc)
            where TEntity : class
        {
            string command = desc ? "ThenByDescending" : "ThenBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property ?? throw new InvalidOperationException());
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
            return (IOrderedQueryable<TEntity>)source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}
