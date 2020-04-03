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
        /// <inheritdoc/>
        public virtual T Get<T>(string key)
        {
            return default(T);
        }

        /// <inheritdoc/>
        public virtual void Set(string key, object data, int cacheTime)
        {
        }

        /// <inheritdoc/>
        public bool IsSet(string key)
        {
            return false;
        }

        /// <inheritdoc/>
        public virtual void Remove(string key)
        {
        }

        /// <inheritdoc/>
        public virtual void RemoveByPattern(string pattern)
        {
        }

        /// <inheritdoc/>
        public virtual void Clear()
        {
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            // nothing special
        }
    }
}
