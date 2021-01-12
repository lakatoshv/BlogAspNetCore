using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using Blog.Core.Consts;
using Blog.Web.Contracts.V1;

namespace Blog.Web.StartupConfigureServicesInstallers
{
    /// <summary>
    /// Swagger installer.
    /// </summary>
    /// <seealso cref="IInstaller" />
    public class SwaggerInstaller : IInstaller
    {
        /// <inheritdoc cref="IInstaller"/>
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Swagger Configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ApiRoutes.Version, new OpenApiInfo
                {
                    Version = ApiRoutes.Version,
                    Title = Consts.ApplicationName,
                    Description = SwaggerConsts.Description,
                    TermsOfService = new Uri(SwaggerConsts.TermsOfService),
                    Contact = new OpenApiContact
                    {
                        Name = SwaggerConsts.Contact.Name,
                        Email = SwaggerConsts.Contact.Email,
                        Url = new Uri(SwaggerConsts.Contact.Url),
                    },
                    License = new OpenApiLicense
                    {
                        Name = SwaggerConsts.License.Name,
                        Url = new Uri(SwaggerConsts.License.Url),
                    }
                });

                c.AddSecurityDefinition(SwaggerConsts.SecurityDefinition.Name, new OpenApiSecurityScheme
                {
                    Description = SwaggerConsts.SecurityDefinition.OpenApiSecurityScheme.Description,
                    Name = SwaggerConsts.SecurityDefinition.OpenApiSecurityScheme.Name,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = SwaggerConsts.SecurityDefinition.OpenApiSecurityScheme.Scheme,
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = SwaggerConsts.SecurityRequirement.OpenApiSecurityRequirement.OpenApiReference.Id,
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });
        }
    }
}