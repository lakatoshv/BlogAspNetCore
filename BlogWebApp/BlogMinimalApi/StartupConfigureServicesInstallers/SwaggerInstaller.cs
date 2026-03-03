namespace BlogMinimalApi.StartupConfigureServicesInstallers
{
    using Asp.Versioning;
    using Blog.Contracts.V1;
    using Blog.Core.Consts;
    using Filters.SwaggerFilters;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi;
    using Swashbuckle.AspNetCore.Filters;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;


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
            services
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new HeaderApiVersionReader("x-api-version"),
                        new MediaTypeApiVersionReader("x-api-version"));
                })
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });
            //services.ConfigureOptions<ConfigureSwaggerOptions>();
            services.AddSwaggerGen(c =>
            {
                c.ExampleFilters();

                c.SwaggerDoc("v1", new OpenApiInfo
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

                /*c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = SwaggerConstants.SecurityRequirement.OpenApiSecurityRequirement.OpenApiReference.Id,
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });*/

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.OperationFilter<ApplySummariesOperationFilter>();
            });

            services.AddSwaggerExamplesFromAssemblyOf<Program>();
        }
    }
}