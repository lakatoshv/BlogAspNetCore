// <copyright file="ISpecification.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Data.Specifications.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Specification interface.
    /// </summary>
    /// <typeparam name="T">T.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        Expression<Func<T, bool>> Filter { get; }

        /// <summary>
        /// Gets the includes.
        /// </summary>
        /// <value>
        /// The includes.
        /// </value>
        List<Expression<Func<T, object>>> Includes { get;  }
    }
}