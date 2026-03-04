namespace BlogMinimalApi.StartupConfigures;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Blog.Core.Infrastructure.Middlewares;

/// <summary>
/// Configure base.
/// </summary>
public class ConfigureBase
{
    /// <summary>
    /// Configures the specified application.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="env">The env.</param>
    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

        app.UseStatusCodePagesWithReExecute("/api/errors/{0}");

        // Configures application to serve the index.html file from /wwwroot
        // when you access the server from a browser
        app.UseDefaultFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
            }
        });

        app.UseResponseCaching();

        app.UseETagger();

        app.UseMiddleware(typeof(ErrorHandlingMiddleware));

        app.UseHealthChecks("/api/health-check");
        app.UseHttpsRedirection();
    }
}