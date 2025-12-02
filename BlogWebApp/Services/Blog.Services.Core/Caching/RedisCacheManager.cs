// <copyright file="RedisCacheManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Caching;

using Blog.Core.Configuration;
using Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Redis cache manager.
/// </summary>
public class RedisCacheManager
{
    /// <summary>
    /// Cache manager.
    /// </summary>
    private readonly ICacheManager perRequestCacheManager;

    /// <summary>
    /// Redis connection wrapper.
    /// </summary>
    private readonly IRedisConnectionWrapper connectionWrapper;

    /// <summary>
    /// Database.
    /// </summary>
    private readonly IDatabase db;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedisCacheManager"/> class.
    /// </summary>
    /// <param name="perRequestCacheManager">perRequestCacheManager.</param>
    /// <param name="connectionWrapper">connectionWrapper.</param>
    /// <param name="config">config.</param>
    public RedisCacheManager(
        ICacheManager perRequestCacheManager,
        IRedisConnectionWrapper connectionWrapper,
        BlogConfiguration config)
    {
        if (string.IsNullOrEmpty(config.RedisCachingConnectionString))
        {
            throw new Exception("Redis connection string is empty");
        }

        this.perRequestCacheManager = perRequestCacheManager;

        // ConnectionMultiplexer.Connect should only be called once and shared between callers
        this.connectionWrapper = connectionWrapper;

        this.db = this.connectionWrapper.GetDatabase();
    }

    /// <summary>
    /// Get data from redis cache.
    /// </summary>
    /// <typeparam name="T">T.</typeparam>
    /// <param name="key">key.</param>
    /// <returns>Type.</returns>
    public virtual T Get<T>(string key)
    {
        return this.GetAsync<T>(key).Result;
    }

    /// <summary>
    /// Set data to redis cache.
    /// </summary>
    /// <param name="key">key.</param>
    /// <param name="data">data.</param>
    /// <param name="cacheTime">cacheTime.</param>
    public virtual async Task Set(string key, object data, int cacheTime)
    {
        await this.SetAsync(key, data, cacheTime);
    }

    /// <summary>
    /// Check if data is set in redis cache by key.
    /// </summary>
    /// <param name="key">key.</param>
    /// <returns>bool.</returns>
    public virtual bool IsSet(string key)
    {
        return this.IsSetAsync(key).Result;
    }

    /// <summary>
    /// Remove data from redis cache.
    /// </summary>
    /// <param name="key">key.</param>
    public virtual async Task Remove(string key)
    {
        await this.RemoveAsync(key);
    }

    /// <summary>
    /// Remove data from redis cache by pattern.
    /// </summary>
    /// <param name="pattern">pattern.</param>
    public virtual async Task RemoveByPattern(string pattern)
    {
        await this.RemoveByPatternAsync(pattern);
    }

    /// <summary>
    /// Clear redis cache.
    /// </summary>
    public virtual async Task Clear()
    {
        await this.ClearAsync();
    }

    /// <summary>
    /// Dispose redis cache.
    /// </summary>
    public virtual void Dispose()
    {
        // if (_connectionWrapper != null)
        //    _connectionWrapper.Dispose();
    }

    /// <summary>
    /// Get data from cache manager.
    /// </summary>
    /// <typeparam name="T">T.</typeparam>
    /// <param name="key">key.</param>
    /// <returns>Task.</returns>
    protected virtual async Task<T> GetAsync<T>(string key)
    {
        // little performance workaround here:
        // we use "PerRequestCacheManager" to cache a loaded object in memory for the current HTTP request.
        // this way we won't connect to Redis server many times per HTTP request (e.g. each time to load a locale or setting)
        if (this.perRequestCacheManager.IsSet(key))
        {
            return this.perRequestCacheManager.Get<T>(key);
        }

        // get serialized item from cache
        var serializedItem = await this.db.StringGetAsync(key);
        if (!serializedItem.HasValue)
        {
            return default;
        }

        // deserialize item
        var item = JsonConvert.DeserializeObject<T>(serializedItem);
        if (item == null)
        {
            return default;
        }

        // set item in the per-request cache
        this.perRequestCacheManager.Set(key, item, 0);

        return item;
    }

    /// <summary>
    /// Async set data to cache manager.
    /// </summary>
    /// <param name="key">key.</param>
    /// <param name="data">data.</param>
    /// <param name="cacheTime">cacheTime.</param>
    /// <returns>Task.</returns>
    protected virtual async Task SetAsync(string key, object data, int cacheTime)
    {
        if (data == null)
        {
            return;
        }

        // set cache time
        var expiresIn = TimeSpan.FromMinutes(cacheTime);

        // serialize item
        var serializedItem = JsonConvert.SerializeObject(data);

        // and set it to cache
        await this.db.StringSetAsync(key, serializedItem, expiresIn);
    }

    /// <summary>
    /// Check if data is set in cache manager by key.
    /// </summary>
    /// <param name="key">key.</param>
    /// <returns>Task.</returns>
    protected virtual async Task<bool> IsSetAsync(string key)
    {
        // little performance workaround here:
        // we use "PerRequestCacheManager" to cache a loaded object in memory for the current HTTP request.
        // this way we won't connect to Redis server many times per HTTP request (e.g. each time to load a locale or setting)
        if (this.perRequestCacheManager.IsSet(key))
        {
            return true;
        }

        return await this.db.KeyExistsAsync(key);
    }

    /// <summary>
    /// Remove data from redis cache by key.
    /// </summary>
    /// <param name="key">key.</param>
    /// <returns>Task.</returns>
    protected virtual async Task RemoveAsync(string key)
    {
        // we should always persist the data protection key list
        if (key.Equals(CachingDefaults.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        // remove item from caches
        await this.db.KeyDeleteAsync(key);
        this.perRequestCacheManager.Remove(key);
    }

    /// <summary>
    /// Async remove data from redis cache by pattern.
    /// </summary>
    /// <param name="pattern">pattern.</param>
    /// <returns>Task.</returns>
    protected virtual async Task RemoveByPatternAsync(string pattern)
    {
        this.perRequestCacheManager.RemoveByPattern(pattern);

        foreach (var endPoint in this.connectionWrapper.GetEndPoints())
        {
            var server = this.connectionWrapper.GetServer(endPoint);
            var keys = server.Keys(database: this.db.Database, pattern: $"*{pattern}*");

            // we should always persist the data protection key list
            keys = keys.Where(key => !key.ToString().Equals(CachingDefaults.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase));

            await this.db.KeyDeleteAsync(keys.ToArray());
        }
    }

    /// <summary>
    /// Async clear redis cache data.
    /// </summary>
    /// <returns>Task.</returns>
    protected virtual async Task ClearAsync()
    {
        this.perRequestCacheManager.Clear();

        foreach (var endPoint in this.connectionWrapper.GetEndPoints())
        {
            var server = this.connectionWrapper.GetServer(endPoint);

            // we can use the code below (commented), but it requires administration permission - ",allowAdmin=true"
            // server.FlushDatabase();

            // that's why we manually delete all elements
            var keys = server.Keys(database: this.db.Database);

            // we should always persist the data protection key list
            keys = keys.Where(key => !key.ToString().Equals(CachingDefaults.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase));

            await this.db.KeyDeleteAsync(keys.ToArray());
        }
    }
}