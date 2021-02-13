namespace Blog.Services.CacheServices
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Response cache service interface.
    /// </summary>
    public interface IResponseCacheService
    {
        /// <summary>
        /// Caches the response asynchronous.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="response">The response.</param>
        /// <param name="lifeTime">The life time.</param>
        /// <returns>Task.</returns>
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan lifeTime);

        /// <summary>
        /// Gets the cached response asynchronous.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>Task.</returns>
        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}