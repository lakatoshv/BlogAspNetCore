namespace Blog.Web.StartupConfigures;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Configure spa.
/// </summary>
public static class ConfigureSpa
{
    /// <summary>
    /// Configures the specified application.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="env">The env.</param>
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSpa(spa =>
        {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501

            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment())
            {
                spa.UseProxyToSpaDevelopmentServer("http://localhost:4200"); ;
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