// <copyright file="ProfileSpecification.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Data.Specifications;

using Base;
using Models;
using System;
using System.Linq.Expressions;

/// <summary>
/// Profile specification.
/// </summary>
/// <seealso cref="BaseSpecification{Profile}" />
public class ProfileSpecification : BaseSpecification<Profile>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileSpecification"/> class.
    /// </summary>
    /// <param name="filter">The filter.</param>
    public ProfileSpecification(Expression<Func<Profile, bool>> filter)
        : base(filter)
    {
    }
}