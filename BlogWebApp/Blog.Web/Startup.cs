namespace Blog.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Blog.Core.Configuration;
    using Microsoft.Extensions.Logging;
    using System.Net;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using System.IO;
    using Microsoft.AspNetCore.Rewrite;
    using Blog.Web.StartupConfigureServicesInstallers;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Startup program configuration.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesInAssembly(Configuration);
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // TODO: Add development configuration
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHealthChecks("/api/health-check");

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });

            app.UseAuthentication();

            // TODO: Implement more advanced Error Handling
            // TODO: Extract implementations to external files
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        //context.Response.AddApplicationError(error.Error.Message);
                        await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                    }
                });
            });


            // Redirect any non-API calls to the Angular application
            // so our application can handle the routing
            // TODO: Extract to extension method .AddAngular()
            app.Use(async (context, next) =>
            {
                await next();

                if (
                    context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/")
                    )
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            // Configure application for usage as API
            // with default route of '/api/[Controller]'

            app.UseMvcWithDefaultRoute();
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHealthChecks("/health");
                endpoints.MapRazorPages();
            });


            // Configures application to serve the index.html file from /wwwroot
            // when you access the server from a browser
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseCors("EnableCORS");

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
                else
                {
                    RedirectFromHttpToHttps(app);
                }
            });
        }

        /// <summary>
        /// Redirects from HTTP to HTTPS.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        private static void RedirectFromHttpToHttps(IApplicationBuilder applicationBuilder)
        {
            var options = new RewriteOptions().AddRedirectToHttps();
            applicationBuilder.UseRewriter(options);
        }
    }
}
