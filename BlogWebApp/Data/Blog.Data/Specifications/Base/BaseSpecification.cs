// <copyright file="BaseSpecification.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Data.Specifications.Base;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

/// <summary>
/// Base specification.
/// </summary>
/// <typeparam name="T">T.</typeparam>
/// <seealso cref="ISpecification{T}" />
public class BaseSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class.
    /// </summary>
    public BaseSpecification()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseSpecification{T}"/> class.
    /// </summary>
    /// <param name="filter">The filter.</param>
    public BaseSpecification(
        Expression<Func<T, bool>> filter)
    {
        this.Filter = filter;
    }

    /// <summary>
    /// Gets the filter.
    /// </summary>
    /// <value>
    /// The filter.
    /// </value>
    public Expression<Func<T, bool>> Filter { get; }

    /// <summary>
    /// Gets the includes.
    /// </summary>
    /// <value>
    /// The includes.
    /// </value>
    public List<Expression<Func<T, object>>> Includes { get; } = new ();

    /// <summary>
    /// Adds the Include.
    /// </summary>
    /// <param name="expression">The expression.</param>
    protected void AddInclude(Expression<Func<T, object>> expression) =>
        this.Includes.Add(expression);

    /// <summary>
    /// Adds the Include.
    /// </summary>
    /// <param name="expressions">The expressions.</param>
    protected void AddInclude(List<Expression<Func<T, object>>> expressions) =>
        this.Includes.AddRange(expressions);
}