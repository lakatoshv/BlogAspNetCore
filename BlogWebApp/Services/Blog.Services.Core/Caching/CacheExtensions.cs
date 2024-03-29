﻿// <copyright file="CacheExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Caching;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Blog.Core.Consts;
using Interfaces;

/// <summary>
/// Cache extensions.
/// </summary>
public static class CacheExtensions
{
    /// <summary>
    /// Get from Cache.
    /// </summary>
    /// <typeparam name="T">T.</typeparam>
    /// <param name="cacheManager">cacheManager.</param>
    /// <param name="key">key.</param>
    /// <param name="acquire">acquire.</param>
    /// <returns>Type.</returns>
    public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
    {
        // use default cache time
        return Get(cacheManager, key, Consts.DefaultCacheTimeMinutes, acquire);
    }

    /// <summary>
    /// Get from Cache.
    /// </summary>
    /// <typeparam name="T">T.</typeparam>
    /// <param name="cacheManager">cacheManager.</param>
    /// <param name="key">key.</param>
    /// <param name="cacheTime">cacheTime.</param>
    /// <param name="acquire">acquire.</param>
    /// <returns>Type.</returns>
    public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
    {
        // item already is in cache, so return it
        if (cacheManager.IsSet(key))
        {
            return cacheManager.Get<T>(key);
        }

        // or create it using passed function
        var result = acquire();

        // and set in cache (if cache time is defined)
        if (cacheTime > 0)
        {
            cacheManager.Set(key, result, cacheTime);
        }

        return result;
    }

    /// <summary>
    /// Remove from Cache by pattern.
    /// </summary>
    /// <param name="cacheManager">cacheManager.</param>
    /// <param name="pattern">pattern.</param>
    /// <param name="keys">keys.</param>
    public static void RemoveByPattern(this ICacheManager cacheManager, string pattern, IEnumerable<string> keys)
    {
        // get cache keys that matches pattern
        var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var matchesKeys = keys.Where(key => regex.IsMatch(key)).ToList();

        // remove matching values
        matchesKeys.ForEach(cacheManager.Remove);
    }
}