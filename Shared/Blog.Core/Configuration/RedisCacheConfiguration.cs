// <copyright file="RedisCacheConfiguration.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Configuration;

/// <summary>
/// Redis cache configuration.
/// </summary>
public class RedisCacheConfiguration
{
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="RedisCacheConfiguration"/> is enabled.
    /// </summary>
    /// <value>
    ///   <c>true</c> if enabled; otherwise, <c>false</c>.
    /// </value>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    /// <value>
    /// The connection string.
    /// </value>
    public string ConnectionString { get; set; }
}