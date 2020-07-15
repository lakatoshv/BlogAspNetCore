using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Blog.Web.StartupConfigureServicesInstallers
{
    /// <summary>
    /// Swagger installer.
    /// </summary>
    /// <seealso cref="IInstaller" />
    public class SwaggerInstaller : IInstaller
    {
        /// <inheritdoc />
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Swagger Configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Blog Web Api",
                    Description = "Blog Web Api Endpoints",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Vitalii Lakatosh",
                        Email = string.Empty,
                        Url = new Uri("http://lakatoshv.byethost8.com/resume.php"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });
        }
    }
}