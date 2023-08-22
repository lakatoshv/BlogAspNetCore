// <copyright file="IRedisConnectionWrapper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Caching.Interfaces;

using System;
using System.Net;
using StackExchange.Redis;

/// <summary>
/// Redis connection wrapper interface.
/// </summary>
public interface IRedisConnectionWrapper : IDisposable
{
    /// <summary>
    /// Get database.
    /// </summary>
    /// <param name="db">db.</param>
    /// <returns>IDatabase.</returns>
    IDatabase GetDatabase(int? db = null);

    /// <summary>
    /// Get server.
    /// </summary>
    /// <param name="endPoint">endPoint.</param>
    /// <returns>IServer.</returns>
    IServer GetServer(EndPoint endPoint);

    /// <summary>
    /// Get EndPoints.
    /// </summary>
    /// <returns>EndPoint[].</returns>
    EndPoint[] GetEndPoints();

    /// <summary>
    /// Flush database.
    /// </summary>
    /// <param name="db">db.</param>
    void FlushDatabase(int? db = null);
}