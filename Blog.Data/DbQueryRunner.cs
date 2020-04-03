// <copyright file="DbQueryRunner.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data
{
    using System;
    using Core;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Database query runner.
    /// </summary>
    public class DbQueryRunner : IDbQueryRunner
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbQueryRunner"/> class.
        /// </summary>
        /// <param name="context">context.</param>
        public DbQueryRunner(ApplicationDbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets or sets context.
        /// </summary>
        public ApplicationDbContext Context { get; set; }

        /// <inheritdoc/>
        public void RunQuery(string query, params object[] parameters)
        {
            this.Context.Database.ExecuteSqlCommand(query, parameters);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Context?.Dispose();
        }
    }
}
