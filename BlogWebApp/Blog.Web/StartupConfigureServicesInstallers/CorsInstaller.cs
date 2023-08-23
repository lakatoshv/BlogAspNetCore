namespace Blog.Web.StartupConfigureServicesInstallers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

/// <summary>
/// Cors installer.
/// </summary>
/// <seealso cref="IInstaller" />
public class CorsInstaller : IInstaller
{
    /// <inheritdoc cref="IInstaller"/>
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration.GetSection("Cors")?.GetSection("Origins")?.Value?.Split(',');
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                if (origins == null || origins.Length <= 0)
                {
                    return;
                }

                if (origins.Contains("*"))
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.SetIsOriginAllowed(host => true);
                    builder.AllowCredentials();
                }
                else
                {
                    foreach (var origin in origins)
                    {
                        builder.WithOrigins(origin);
                    }
                }
            });
        });
    }
}