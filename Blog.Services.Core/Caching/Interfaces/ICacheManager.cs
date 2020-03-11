namespace Blog.Services.Core.Caching.Interfaces
{
    using System;
    using Blog.Core.Consts;

    public interface ICacheManager : IDisposable
    {
        T Get<T>(string key);

        void Set(string key, object data, int cacheTime = Consts.DefaultCacheTimeMinutes);

        bool IsSet(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);

        void Clear();
    }
}
