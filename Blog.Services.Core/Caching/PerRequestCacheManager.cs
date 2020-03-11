namespace Blog.Services.Core.Caching
{
    using System.Collections.Generic;
    using System.Linq;
    using Blog.Services.Core.Caching.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class PerRequestCacheManager : ICacheManager
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor
        public PerRequestCacheManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Utilities
        protected virtual IDictionary<object, object> GetItems()
        {
            return _httpContextAccessor.HttpContext?.Items;
        }

        #endregion

        #region Methods
        public virtual T Get<T>(string key)
        {
            var items = GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (data != null)
                items[key] = data;
        }

        public virtual bool IsSet(string key)
        {
            var items = GetItems();

            return items?[key] != null;
        }

        public virtual void Remove(string key)
        {
            var items = GetItems();

            items?.Remove(key);
        }

        public virtual void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            this.RemoveByPattern(pattern, items.Keys.Select(p => p.ToString()));
        }

        public virtual void Clear()
        {
            var items = GetItems();

            items?.Clear();
        }

        public virtual void Dispose()
        {
            //nothing special
        }

        #endregion
    }
}
