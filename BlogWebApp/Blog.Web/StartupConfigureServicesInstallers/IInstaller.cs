namespace Blog.Web.StartupConfigureServicesInstallers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Installer interface.
/// </summary>
public interface IInstaller
{
    /// <summary>
    /// Installs the services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    void InstallServices(IServiceCollection services, IConfiguration configuration);
}