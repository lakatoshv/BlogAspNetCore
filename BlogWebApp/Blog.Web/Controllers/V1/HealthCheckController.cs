﻿using System;
using Blog.Contracts.V1.Responses.HealthChecks;
using Blog.Core.Consts;

namespace Blog.Web.Controllers.V1
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Blog.Contracts.V1;
    using Blog.Services.ControllerContext;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    /// <summary>
    /// Health check controller.
    /// Analyze API on healthy.
    /// </summary>
    /// <seealso cref="Blog.Web.Controllers.BaseController" />
    [Route(ApiRoutes.HealthController.Health)]
    [ApiController]
    [AllowAnonymous]
    [Produces(Consts.JsonType)]
    public class HealthCheckController : BaseController
    {
        /// <summary>
        /// The health check service.
        /// </summary>
        private readonly HealthCheckService healthCheckService;
        public HealthCheckController(
            IControllerContext workContext,
            HealthCheckService healthCheckService) : base(workContext)
        {
            this.healthCheckService = healthCheckService;
        }
        
        /// <summary>
        /// Get health check information.
        /// </summary>
        /// <returns>HealthStatus json.</returns>
        /// <response code="200">API is healthy.</response>
        /// <response code="503">API is unhealthy.</response>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var report = await this.healthCheckService.CheckHealthAsync();

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

                return report.Status == HealthStatus.Healthy ? this.Ok(response) : this.StatusCode((int)HttpStatusCode.ServiceUnavailable, response);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }
    }
}
