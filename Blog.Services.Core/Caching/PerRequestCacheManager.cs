// <copyright file="PerRequestCacheManager.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Caching
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Per request cache manager.
    /// </summary>
    public class PerRequestCacheManager : ICacheManager
    {
        /// <summary>
        /// Http context accessor.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerRequestCacheManager"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">httpContextAccessor.</param>
        public PerRequestCacheManager(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc cref="ICacheManager"/>
        public virtual T Get<T>(string key)
        {
            var items = this.GetItems();
            if (items == null)
            {
                return default(T);
            }

            return (T)items[key];
        }

        /// <inheritdoc cref="ICacheManager"/>
        public virtual void Set(string key, object data, int cacheTime)
        {
            var items = this.GetItems();
            if (items == null)
            {
                return;
            }

            if (data != null)
            {
                items[key] = data;
            }
        }

        /// <inheritdoc cref="ICacheManager"/>
        public virtual bool IsSet(string key)
        {
            var items = this.GetItems();

            return items?[key] != null;
        }

        /// <inheritdoc cref="ICacheManager"/>
        public virtual void Remove(string key)
        {
            var items = this.GetItems();

            items?.Remove(key);
        }

        /// <inheritdoc cref="ICacheManager"/>
        public virtual void RemoveByPattern(string pattern)
        {
            var items = this.GetItems();
            if (items == null)
            {
                return;
            }

            this.RemoveByPattern(pattern, items.Keys.Select(p => p.ToString()));
        }

        /// <inheritdoc cref="ICacheManager"/>
        public virtual void Clear()
        {
            var items = this.GetItems();

            items?.Clear();
        }

        /// <inheritdoc cref="ICacheManager"/>
        public virtual void Dispose()
        {
            // nothing special
        }

        /// <summary>
        /// Get Items.
        /// </summary>
        /// <returns>IDictionary.</returns>
        protected virtual IDictionary<object, object> GetItems()
        {
            return this._httpContextAccessor.HttpContext?.Items;
        }
    }
}
