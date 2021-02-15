// <copyright file="ResponseCacheService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.CacheServices
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Distributed;
    using Newtonsoft.Json;

    /// <summary>
    /// Response cache service.
    /// </summary>
    /// <seealso cref="IResponseCacheService" />
    public class ResponseCacheService : IResponseCacheService
    {
        /// <summary>
        /// The distributed cache.
        /// </summary>
        private readonly IDistributedCache distributedCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseCacheService"/> class.
        /// </summary>
        /// <param name="distributedCache">The distributed cache.</param>
        public ResponseCacheService(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        /// <inheritdoc cref="IResponseCacheService"/>
        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan lifeTime)
        {
            if (response == null)
            {
                return;
            }

            var serializedResponse = JsonConvert.SerializeObject(response);

            await this.distributedCache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = lifeTime,
            });
        }

        /// <inheritdoc cref="IResponseCacheService"/>
        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await this.distributedCache.GetStringAsync(cacheKey);

            return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
        }
    }
}