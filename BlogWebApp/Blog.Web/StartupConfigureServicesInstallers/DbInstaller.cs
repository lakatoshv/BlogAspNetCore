namespace Blog.Web.StartupConfigureServicesInstallers
{
    using Blog.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
        }
    }
}