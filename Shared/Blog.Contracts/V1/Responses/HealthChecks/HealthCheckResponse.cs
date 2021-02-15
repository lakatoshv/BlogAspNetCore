using System;
using System.Collections.Generic;

namespace Blog.Contracts.V1.Responses.HealthChecks
{
    /// <summary>
    /// Health check response.
    /// </summary>
    public class HealthCheckResponse
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the checks.
        /// </summary>
        /// <value>
        /// The checks.
        /// </value>
        public IEnumerable<HealthCheck> Checks { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration { get; set; }
    }
}