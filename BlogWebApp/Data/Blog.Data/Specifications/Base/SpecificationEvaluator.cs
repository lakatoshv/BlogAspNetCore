// <copyright file="SpecificationEvaluator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Data.Specifications.Base;

using System.Linq;
using Blog.Core;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Specification evaluator.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public class SpecificationEvaluator<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Gets the query.
    /// </summary>
    /// <param name="inputQuery">The input query.</param>
    /// <param name="specification">The specification.</param>
    /// <returns>IQueryable.</returns>
    public static IQueryable<TEntity> GetQuery(
        IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> specification)
    {
        if (specification.Filter != null)
        {
            inputQuery = inputQuery.Where(specification.Filter);
        }

        return specification.Includes.Aggregate(inputQuery, (current, includes) => current.Include(includes));
    }
}