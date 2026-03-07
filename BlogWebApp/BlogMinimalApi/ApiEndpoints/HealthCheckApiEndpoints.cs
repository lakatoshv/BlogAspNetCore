using Asp.Versioning;
using Blog.Contracts.V1;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BlogMinimalApi.ApiEndpoints;

/// <summary>
/// Accounts Api endpoints.
/// </summary>
public class HealthCheckApiEndpoints : IRoutesInstaller
{
    public void InstallApiRoutes(WebApplication app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup(ApiRoutes.HealthController.Health)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithTags("Accounts");

        // -------------------------
        // PUBLIC ENDPOINTS
        // -------------------------

        var publicGroup = group.AllowAnonymous();

        // Initialize (roles list)
        publicGroup.MapGet("/health", async (HealthCheckService healthCheckService) =>
            {
                var report = await healthCheckService.CheckHealthAsync();

                var response = new
                {
                    Status = report.Status.ToString(),
                    Checks = report.Entries.Select(x => new
                    {
                        Component = x.Key,
                        Status = x.Value.Status.ToString(),
                        Description = x.Value.Description
                    }),
                    Duration = report.TotalDuration
                };

                return report.Status == HealthStatus.Healthy
                    ? Results.Ok(response)
                    : Results.Problem(
                        detail: "Service is unhealthy",
                        statusCode: StatusCodes.Status503ServiceUnavailable);
            })
            .WithName("Health")
            .AllowAnonymous();
    }
}