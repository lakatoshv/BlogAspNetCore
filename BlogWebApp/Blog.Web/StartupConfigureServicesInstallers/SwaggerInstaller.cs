using System.Linq;
using Blog.Web.Filters.SwaggerFilters;

namespace Blog.Web.StartupConfigureServicesInstallers
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Blog.Core.Consts;
    using Blog.Contracts.V1;
    using Swashbuckle.AspNetCore.Filters;

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
                c.ExampleFilters();

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

                c.ResolveConflictingActions(x => x.First());

                var url = configuration.GetSection("IdentityServer").GetValue<string>("Url");

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

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.OperationFilter<ApplySummariesOperationFilter>();
            });

            services.AddSwaggerExamplesFromAssemblyOf<Program>();
        }
    }
}