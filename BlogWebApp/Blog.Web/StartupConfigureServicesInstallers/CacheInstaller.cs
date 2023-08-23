namespace Blog.Web.StartupConfigureServicesInstallers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Configuration;

/// <summary>
/// Cache installer.
/// </summary>
/// <seealso cref="IInstaller" />
public class CacheInstaller : IInstaller
{
    /// <summary>
    /// Installs the services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        var redisCacheSettings = new RedisCacheConfiguration();
        configuration.GetSection(nameof(RedisCacheConfiguration)).Bind(redisCacheSettings);
        services.AddSingleton(redisCacheSettings);

        if (!redisCacheSettings.Enabled)
        {
            return;
        }

        // services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisCacheSettings.ConnectionString));
        services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
    }
}