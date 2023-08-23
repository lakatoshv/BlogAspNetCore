// <copyright file="PostSpecification.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Data.Specifications;

using System;
using System.Linq.Expressions;
using Models;
using Base;

/// <summary>
/// Post specification.
/// </summary>
/// <seealso cref="BaseSpecification{Post}" />
public class PostSpecification : BaseSpecification<Post>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostSpecification"/> class.
    /// </summary>
    public PostSpecification()
    {
        this.AddInclude(x => x.Author);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PostSpecification"/> class.
    /// </summary>
    /// <param name="filter">The filter.</param>
    public PostSpecification(Expression<Func<Post, bool>> filter)
        : base(filter)
    {
    }
}