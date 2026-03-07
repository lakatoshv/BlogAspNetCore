namespace BlogMinimalApi.ApiEndpoints;

/// <summary>
/// Installer interface.
/// </summary>
public interface IRoutesInstaller
{
    /// <summary>
    /// Installs the Api routes.
    /// </summary>
    /// <param name="app">The app.</param>
    void InstallApiRoutes(WebApplication app);
}