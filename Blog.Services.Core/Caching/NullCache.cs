// <copyright file="NullCache.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Caching
{
    using Interfaces;

    /// <summary>
    /// Null cache.
    /// </summary>
    public class NullCache : IStaticCacheManager
    {
        /// <inheritdoc cref="ICacheManager"/>
        public virtual T Get<T>(string key)
        {
            return default(T);
        }

        /// <inheritdoc cref="ICacheManager"/>
        public virtual void Set(string key, object data, int cacheTime)
        {
        }

        /// <inheritdoc cref="ICacheManager"/>
        public bool IsSet(string key)
        {
            return false;
        }

        /// <inheritdoc cref="ICacheManager"/>
        public virtual void Remove(string key)
        {
        }

        /// <inheritdoc cref="ICacheManager"/>
        public virtual void RemoveByPattern(string pattern)
        {
        }

        /// <inheritdoc cref="ICacheManager"/>
        public virtual void Clear()
        {
        }

        /// <inheritdoc cref="ICacheManager"/>
        public virtual void Dispose()
        {
            // nothing special
        }
    }
}
