using System;
using Blog.Data;
using Blog.Web.HealthChecks;
using HealthChecks.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Blog.Web.StartupConfigureServicesInstallers
{
    public class HealthCheckInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>()
                .AddCheck<RedisHealthCheck>("Redis");
        }
    }
}