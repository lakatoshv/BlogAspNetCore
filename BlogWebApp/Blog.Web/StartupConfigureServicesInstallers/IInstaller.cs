using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Web.StartupConfigureServicesInstallers
{
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
}