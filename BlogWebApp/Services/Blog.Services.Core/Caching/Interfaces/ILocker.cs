// <copyright file="ILocker.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Caching.Interfaces;

using System;

/// <summary>
/// Locker interface.
/// </summary>
public interface ILocker
{
    /// <summary>
    /// Perform action with lock.
    /// </summary>
    /// <param name="resource">resource.</param>
    /// <param name="expirationTime">expirationTime.</param>
    /// <param name="action">action.</param>
    /// <returns>bool.</returns>
    bool PerformActionWithLock(string resource, TimeSpan expirationTime, Action action);
}