using Blog.Data;
using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Web.StartupConfigureServicesInstallers
{
    /// <summary>
    /// Identity installer.
    /// </summary>
    /// <seealso cref="IInstaller" />
    public class IdentityInstaller : IInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Add Identity 
            // TODO: Extract to external extension method .AddIdentity()
            services.AddScoped<RoleManager<ApplicationRole>>();

            var builder = services.AddIdentityCore<ApplicationUser>(o =>
            {
                // Configure Identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });

            /*
            services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserStore<ApplicationUserStore>()
                .AddRoleStore<ApplicationRoleStore>()
                .AddDefaultTokenProviders();
            */


            builder = new IdentityBuilder(builder.UserType, typeof(ApplicationRole), builder.Services);

            builder.AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}