// <copyright file="QueryableMappingExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Mapping
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using AutoMapper.QueryableExtensions;

    /// <summary>
    /// Queryable mapping extensions.
    /// </summary>
    public static class QueryableMappingExtensions
    {
        /// <summary>
        /// To map.
        /// </summary>
        /// <typeparam name="TDestination">TDestination.</typeparam>
        /// <param name="source">source.</param>
        /// <param name="membersToExpand">membersToExpand.</param>
        /// <returns>IQueryable.</returns>
        public static IQueryable<TDestination> To<TDestination>(
            this IQueryable source,
            params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo(membersToExpand);
        }

        /// <summary>
        /// To map.
        /// </summary>
        /// <typeparam name="TDestination">TDestination.</typeparam>
        /// <param name="source">source.</param>
        /// <param name="parameters">parameters.</param>
        /// <returns>IQueryable.</returns>
        public static IQueryable<TDestination> To<TDestination>(
            this IQueryable source,
            object parameters)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo<TDestination>(parameters);
        }
    }
}
