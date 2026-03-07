namespace BlogMinimalApi.ApiEndpoints;

/// <summary>
/// Installer extensions.
/// </summary>
public static class RouterInstallerExtensions
{
    /// <summary>
    /// Installs the Api routes in assembly.
    /// </summary>
    public static void InstallApiRoutesInAssembly(this WebApplication app)
    {
        var installers = typeof(Program).Assembly.ExportedTypes.Where(x =>
                typeof(IRoutesInstaller).IsAssignableFrom(x)
                && x is { IsInterface: false, IsAbstract: false })
            .Select(Activator.CreateInstance)
            .Cast<IRoutesInstaller>().ToList();

        installers.ForEach(installer => installer.InstallApiRoutes(app));
    }
}