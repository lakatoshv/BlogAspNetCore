namespace Blog.Web.Cache;

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using CommonServices.Interfaces;
using Core.Configuration;

/// <summary>
/// Cached attribute.
/// </summary>
/// <seealso cref="Attribute" />
/// <seealso cref="IAsyncActionFilter" />
/// <remarks>
/// Initializes a new instance of the <see cref="CachedAttribute"/> class.
/// </remarks>
/// <param name="lifeTimeSeconds">The lifetime seconds.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CachedAttribute(int lifeTimeSeconds)
    : Attribute, IAsyncActionFilter
{
    /// <summary>
    /// The lifetime seconds.
    /// </summary>
    private readonly int _lifeTimeSeconds = lifeTimeSeconds;

    /// <summary>
    /// Called asynchronously before the action, after model binding is complete.
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
    /// <param name="next">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate" />. Invoked to execute the next action filter or the action itself.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheConfiguration>();
        if (!cacheSettings.Enabled)
        {
            await next();
            return;
        }

        var cachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
        var cachedKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

        var cachedResponse = await cachedService.GetCachedResponseAsync(cachedKey);
        if (!string.IsNullOrEmpty(cachedResponse))
        {
            var contentResult = new ContentResult
            {
                Content = cachedResponse,
                ContentType = "application/json",
                StatusCode = 200,
            };

            context.Result = contentResult;

            return;
        }

        var executedContext = await next();
        if (executedContext.Result is OkObjectResult okObjectResult)
        {
            await cachedService.CacheResponseAsync(cachedKey, okObjectResult.Value, TimeSpan.FromSeconds(_lifeTimeSeconds));
        }
    }

    /// <summary>
    /// Generates the cache key from request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns></returns>
    private static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();
        keyBuilder.Append($"{request.Path}");

        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            keyBuilder.Append($"{key}-{value}");
        }

        return keyBuilder.ToString();
    }
}