namespace Blog.Web.HealthChecks

open System
open System.Threading
open System.Threading.Tasks

open Microsoft.Extensions.Diagnostics.HealthChecks
open Microsoft.Extensions.Logging

type ReadinessPublisher(logger: ILogger<ReadinessPublisher>) =

    interface IHealthCheckPublisher with

        member _.PublishAsync
            (
                report: HealthReport,
                cancellationToken: CancellationToken
            ) : Task =

            if report.Status = HealthStatus.Healthy then
                logger.LogInformation(
                    "{Timestamp} Readiness Probe Status: {Result}",
                    DateTime.UtcNow,
                    report.Status
                )
            else
                logger.LogError(
                    "{Timestamp} Readiness Probe Status: {Result}",
                    DateTime.UtcNow,
                    report.Status
                )

            cancellationToken.ThrowIfCancellationRequested()

            Task.CompletedTask