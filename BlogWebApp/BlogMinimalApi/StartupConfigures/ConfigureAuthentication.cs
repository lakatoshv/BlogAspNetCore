namespace BlogMinimalApi.StartupConfigures;

using Microsoft.AspNetCore.Builder;

/// <summary>
/// Configure authentication.
/// </summary>
public static class ConfigureAuthentication
{
    /// <summary>
    /// Configures the specified application.
    /// </summary>
    /// <param name="app">The application.</param>
    public static void Configure(IApplicationBuilder app)
    {
        app.UseCors("EnableCORS");
        app.UseCors("AllowAllBlazor");
    }
}