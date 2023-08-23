namespace Blog.Web.StartupConfigureServicesInstallers;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Filters;

/// <summary>
/// Mvc installer.
/// </summary>
/// <seealso cref="IInstaller" />
public class MvcInstaller : IInstaller
{
    /// <inheritdoc cref="IInstaller"/>
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMvc(options =>
        {
            options.EnableEndpointRouting = false;
            options.Filters.Add<ValidationFilter>();
        });
        services.AddRazorPages();
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        // In production, the Angular files will be served from this directory
        services.AddSpaStaticFiles(config =>
        {
            config.RootPath = "ClientApp/dist";
        });
    }
}