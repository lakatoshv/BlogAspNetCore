namespace Blog.Web.StartupConfigureServicesInstallers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

/// <summary>
/// ApplicationBaseInstaller.
/// </summary>
/// <seealso cref="IInstaller" />
public class ApplicationBaseInstaller : IInstaller
{
    /// <inheritdoc cref="IInstaller"/>
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        // file upload dependency
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        services.AddAutoMapper(typeof(Program));
        services.AddOptions();
        services.AddLocalization();
        services.AddMemoryCache();
        services.AddResponseCaching();
    }
}