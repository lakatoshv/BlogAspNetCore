namespace Blog.Web.StartupConfigures
{
    using Blog.Core.Configuration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Swashbuckle.AspNetCore.SwaggerUI;

    /// <summary>
    /// Configure swagger.
    /// </summary>
    public static class ConfigureSwagger
    {
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerOptions = new SwaggerOptions();
            configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
                //option.HeadContent = $"{Blog.Web.Assembly.Git.Branch.ToUpper()} {BlogAssembly.Git.Commit.ToUpper()}";
                option.DefaultModelExpandDepth(0);
                option.DefaultModelRendering(ModelRendering.Model);
                option.DefaultModelsExpandDepth(0);
                option.DocExpansion(DocExpansion.None);
                option.DisplayRequestDuration();
            });
        }
    }
}