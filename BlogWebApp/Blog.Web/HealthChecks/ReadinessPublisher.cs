using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Blog.Web.HealthChecks
{
    /// <summary>
    /// Readiness publisher.
    /// </summary>
    /// <seealso cref="IHealthCheckPublisher" />
    public class ReadinessPublisher : IHealthCheckPublisher
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadinessPublisher"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public ReadinessPublisher(ILogger<ReadinessPublisher> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Publishes the provided <paramref name="report" />.
        /// </summary>
        /// <param name="report">The <see cref="T:Microsoft.Extensions.Diagnostics.HealthChecks.HealthReport" />. The result of executing a set of health checks.</param>
        /// <param name="cancellationToken">The <see cref="T:System.Threading.CancellationToken" />.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> which will complete when publishing is complete.
        /// </returns>
        public Task PublishAsync(HealthReport report,
            CancellationToken cancellationToken)
        {
            if (report.Status == HealthStatus.Healthy)
            {
                _logger.LogInformation("{Timestamp} Readiness Probe Status: {Result}",
                    DateTime.UtcNow, report.Status);
            }
            else
            {
                _logger.LogError("{Timestamp} Readiness Probe Status: {Result}",
                    DateTime.UtcNow, report.Status);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }
    }
}