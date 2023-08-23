// <copyright file="TagSpecification.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Data.Specifications;

using System;
using System.Linq.Expressions;
using Models;
using Base;

/// <summary>
/// Tag specification.
/// </summary>
/// <seealso cref="BaseSpecification{Tag}" />
public class TagSpecification : BaseSpecification<Tag>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TagSpecification"/> class.
    /// </summary>
    public TagSpecification()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TagSpecification"/> class.
    /// </summary>
    /// <param name="filter">The filter.</param>
    public TagSpecification(Expression<Func<Tag, bool>> filter)
        : base(filter)
    {
    }
}