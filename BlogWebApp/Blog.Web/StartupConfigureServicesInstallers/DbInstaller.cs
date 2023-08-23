namespace Blog.Web.StartupConfigureServicesInstallers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Data;
using Data.Extensions;

/// <summary>
/// Database installer.
/// </summary>
/// <seealso cref="IInstaller" />
public class DbInstaller : IInstaller
{
    /// <inheritdoc cref="IInstaller"/>
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<ApplicationDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddUnitOfWork<ApplicationDbContext>();
    }
}