// <copyright file="CommentSpecification.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Data.Specifications;

using System;
using System.Linq.Expressions;
using Models;
using Base;

/// <summary>
/// Comment specification.
/// </summary>
/// <seealso cref="BaseSpecification{Comment}" />
public class CommentSpecification : BaseSpecification<Comment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommentSpecification"/> class.
    /// </summary>
    public CommentSpecification()
    {
        this.AddInclude(x => x.User);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentSpecification"/> class.
    /// </summary>
    /// <param name="filter">The filter.</param>
    public CommentSpecification(Expression<Func<Comment, bool>> filter)
        : base(filter)
    {
    }
}