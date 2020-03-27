// <copyright file="IDbQueryRunner.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Core
{
    using System;

    /// <summary>
    /// Database query runner interface.
    /// </summary>
    public interface IDbQueryRunner : IDisposable
    {
        /// <summary>
        /// Run query.
        /// </summary>
        /// <param name="query">query.</param>
        /// <param name="parameters">parameters.</param>
        void RunQuery(string query, params object[] parameters);
    }
}
