// <copyright file="QueryableMappingExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Mapping;

using AutoMapper.QueryableExtensions;
using System;
using System.Linq;
using System.Linq.Expressions;

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
        => source == null
            ? throw new ArgumentNullException(nameof(source))
            : source.ProjectTo(null, membersToExpand);

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
        => source == null
            ? throw new ArgumentNullException(nameof(source))
            : source.ProjectTo<TDestination>((AutoMapper.IConfigurationProvider)parameters);
}