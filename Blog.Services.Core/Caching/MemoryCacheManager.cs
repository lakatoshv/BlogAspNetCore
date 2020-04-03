// <copyright file="MemoryCacheManager.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Caching
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading;
    using Interfaces;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Memory cache manager.
    /// </summary>
    public class MemoryCacheManager : ILocker, IStaticCacheManager
    {
        /// <summary>
        /// Concurrent dictionary.
        /// </summary>
        protected static readonly ConcurrentDictionary<string, bool> AllKeys;

        /// <summary>
        /// Memory cache.
        /// </summary>
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Cancellation token source.
        /// </summary>
        protected CancellationTokenSource CancellationTokenSource;

        /// <summary>
        /// Initializes static members of the <see cref="MemoryCacheManager"/> class.
        /// </summary>
        static MemoryCacheManager()
        {
            AllKeys = new ConcurrentDictionary<string, bool>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheManager"/> class.
        /// </summary>
        /// <param name="cache">cache.</param>
        public MemoryCacheManager(IMemoryCache cache)
        {
            this._cache = cache;
            this.CancellationTokenSource = new CancellationTokenSource();
        }

        /// <inheritdoc/>
        public virtual T Get<T>(string key)
        {
            return this._cache.Get<T>(key);
        }

        /// <inheritdoc/>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data != null)
            {
                this._cache.Set(this.AddKey(key), data, this.GetMemoryCacheEntryOptions(TimeSpan.FromMinutes(cacheTime)));
            }
        }

        /// <inheritdoc/>
        public virtual bool IsSet(string key)
        {
            return this._cache.TryGetValue(key, out object _);
        }

        /// <inheritdoc/>
        public bool PerformActionWithLock(string key, TimeSpan expirationTime, Action action)
        {
            // ensure that lock is acquired
            if (!AllKeys.TryAdd(key, true))
            {
                return false;
            }

            try
            {
                this._cache.Set(key, key, this.GetMemoryCacheEntryOptions(expirationTime));

                // perform action
                action();

                return true;
            }
            finally
            {
                // release lock even if action fails
                this.Remove(key);
            }
        }

        /// <inheritdoc/>
        public virtual void Remove(string key)
        {
            this._cache.Remove(this.RemoveKey(key));
        }

        /// <inheritdoc/>
        public virtual void RemoveByPattern(string pattern)
        {
            this.RemoveByPattern(pattern, AllKeys.Where(p => p.Value).Select(p => p.Key));
        }

        /// <inheritdoc/>
        public virtual void Clear()
        {
            // send cancellation request
            this.CancellationTokenSource.Cancel();

            // releases all resources used by this cancellation token
            this.CancellationTokenSource.Dispose();

            // recreate cancellation token
            this.CancellationTokenSource = new CancellationTokenSource();
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            // nothing special
        }

        /// <summary>
        /// Get memory cache entry option.
        /// </summary>
        /// <param name="cacheTime">cacheTime.</param>
        /// <returns>MemoryCacheEntryOptions.</returns>
        protected MemoryCacheEntryOptions GetMemoryCacheEntryOptions(TimeSpan cacheTime)
        {
            var options = new MemoryCacheEntryOptions()

                // add cancellation token for clear cache
                .AddExpirationToken(new CancellationChangeToken(this.CancellationTokenSource.Token))

                // add post eviction callback
                .RegisterPostEvictionCallback(this.PostEviction);

            // set cache time
            options.AbsoluteExpirationRelativeToNow = cacheTime;

            return options;
        }

        /// <summary>
        /// Add key.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns>string.</returns>
        protected string AddKey(string key)
        {
            AllKeys.TryAdd(key, true);
            return key;
        }

        /// <summary>
        /// Remove key.
        /// </summary>
        /// <param name="key">key.</param>
        /// <returns>string.</returns>
        protected string RemoveKey(string key)
        {
            this.TryRemoveKey(key);
            return key;
        }

        /// <summary>
        /// Try remove key.
        /// </summary>
        /// <param name="key">key.</param>
        protected void TryRemoveKey(string key)
        {
            // try to remove key from dictionary
            if (!AllKeys.TryRemove(key, out bool _))
            {
                // if not possible to remove key from dictionary, then try to mark key as not existing in cache
                AllKeys.TryUpdate(key, false, false);
            }
        }

        /// <summary>
        /// Clear keys.
        /// </summary>
        private void ClearKeys()
        {
            foreach (var key in AllKeys.Where(p => !p.Value).Select(p => p.Key).ToList())
            {
                this.RemoveKey(key);
            }
        }

        /// <summary>
        /// Post eviction.
        /// </summary>
        /// <param name="key">key.</param>
        /// <param name="value">value.</param>
        /// <param name="reason">reason.</param>
        /// <param name="state">state.</param>
        private void PostEviction(object key, object value, EvictionReason reason, object state)
        {
            // if cached item just change, then nothing doing
            if (reason == EvictionReason.Replaced)
            {
                return;
            }

            // try to remove all keys marked as not existing
            this.ClearKeys();

            // try to remove this key from dictionary
            this.TryRemoveKey(key.ToString());
        }
    }
}
