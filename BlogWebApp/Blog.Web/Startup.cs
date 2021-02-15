using Blog.Web.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesInAssembly(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    await HealthCheckHelper.GetHealthCheckResponse(context, report);
                }
            });

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

        private static void RedirectFromHttpToHttps(IApplicationBuilder applicationBuilder)
        {
            var options = new RewriteOptions().AddRedirectToHttps();
            applicationBuilder.UseRewriter(options);
        }
    }
}
