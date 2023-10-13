namespace Blog.Web.StartupConfigureServicesInstallers;

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Installer extensions.
/// </summary>
public static class InstallerExtensions
{
    /// <summary>
    /// Installs the services in assembly.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
    {
        var installers = typeof(Program).Assembly.ExportedTypes.Where(x =>
                typeof(IInstaller).IsAssignableFrom(x)
                && x is { IsInterface: false, IsAbstract: false })
            .Select(Activator.CreateInstance)
            .Cast<IInstaller>().ToList();

        installers.ForEach(installer => installer.InstallServices(services, configuration));

        services.AddSingleton(configuration);

    }
}