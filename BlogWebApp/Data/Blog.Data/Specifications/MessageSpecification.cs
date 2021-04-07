// <copyright file="MessageSpecification.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Data.Specifications
{
    using System;
    using System.Linq.Expressions;
    using Blog.Data.Models;
    using Blog.Data.Specifications.Base;

    /// <summary>
    /// Message specification.
    /// </summary>
    /// <seealso cref="BaseSpecification{Message}" />
    public class MessageSpecification : BaseSpecification<Message>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSpecification"/> class.
        /// </summary>
        public MessageSpecification()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSpecification"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public MessageSpecification(Expression<Func<Message, bool>> filter)
            : base(filter)
        {
        }
    }
}