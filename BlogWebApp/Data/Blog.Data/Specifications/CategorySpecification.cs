// <copyright file="CategorySpecification.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Data.Specifications;

using System;
using System.Linq.Expressions;
using Models;
using Base;

/// <summary>
/// Category specification.
/// </summary>
/// <seealso cref="BaseSpecification{Category}" />
public class CategorySpecification : BaseSpecification<Category>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CategorySpecification"/> class.
    /// </summary>
    public CategorySpecification()
    {
        this.AddInclude(x => x.Categories);
        this.AddInclude(x => x.Posts);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CategorySpecification"/> class.
    /// </summary>
    /// <param name="filter">The filter.</param>
    public CategorySpecification(Expression<Func<Category, bool>> filter)
        : base(filter)
    {
    }
}