// <copyright file="ICacheManager.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Caching.Interfaces
{
    using System;
    using Blog.Core.Consts;

    /// <summary>
    /// Cache manager interface.
    /// </summary>
    public interface ICacheManager : IDisposable
    {
        /// <summary>
        /// Get T from cache.
        /// </summary>
        /// <typeparam name="T">T.</typeparam>
        /// <param name="key">key.</param>
        /// <returns>Type.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Set data to cache.
        /// </summary>
        /// <param name="key">key.</param>
        /// <param name="data">data.</param>
        /// <param name="cacheTime">cacheTime.</param>
        void Set(string key, object data, int cacheTime = Consts.DefaultCacheTimeMinutes);

        /// <summary>
        /// Check if data is set to cache by key.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns>bool.</returns>
        bool IsSet(string key);

        /// <summary>
        /// Remove data from cache by key.
        /// </summary>
        /// <param name="key">key.</param>
        void Remove(string key);

        /// <summary>
        /// Remove data from cache by pattern.
        /// </summary>
        /// <param name="pattern">pattern.</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// Clear cache.
        /// </summary>
        void Clear();
    }
}
