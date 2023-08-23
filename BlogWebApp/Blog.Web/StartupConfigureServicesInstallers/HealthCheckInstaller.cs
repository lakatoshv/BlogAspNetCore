namespace Blog.Web.StartupConfigureServicesInstallers;

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Data;
using HealthChecks;

/// <summary>
/// Health check installer.
/// </summary>
/// <seealso cref="IInstaller" />
public class HealthCheckInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();
        // .AddCheck<RedisHealthCheck>("Redis");

        services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Delay = TimeSpan.FromSeconds(2);
            options.Predicate = (check) => check.Tags.Contains("ready");
        });

        services.AddSingleton<IHealthCheckPublisher, ReadinessPublisher>();
    }
}