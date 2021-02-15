using System.Linq;
using System.Threading.Tasks;
using Blog.Contracts.V1.Responses.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Blog.Web.HealthChecks
{
    /// <summary>
    /// Health check helper.
    /// </summary>
    public class HealthCheckHelper
    {
        /// <summary>
        /// Gets the health check response.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="report">The report.</param>
        public static async Task GetHealthCheckResponse(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";
            var response = new HealthCheckResponse
            {
                Status = report.Status.ToString(),
                Checks = report.Entries.Select(x => new HealthCheck
                {
                    Component = x.Key,
                    Status = x.Value.Status.ToString(),
                    Description = x.Value.Description,
                }),
                Duration = report.TotalDuration,
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}