namespace Blog.Web.StartupConfigures
{
    using System.IO;
    using System.Net;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Configure routes.
    /// </summary>
    public class ConfigureRoutes
    {
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void Configure(IApplicationBuilder app)
        {
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

                if (context.Response.StatusCode != 404 || Path.HasExtension(context.Request.Path.Value) ||
                    context.Request.Path.Value.StartsWith("/api/"))
                {
                }
                else
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
        }
    }
}