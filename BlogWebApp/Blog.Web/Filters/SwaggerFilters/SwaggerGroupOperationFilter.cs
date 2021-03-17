﻿namespace Blog.Web.Filters.SwaggerFilters
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.OpenApi.Models;
    using Blog.Core.Attributes;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Swagger operation filter for <see cref="SwaggerGroupAttribute"/>
    /// </summary>
    public class SwaggerGroupOperationFilter : IOperationFilter
    {
        /// <inheritdoc />
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor
            )
            {
                return;
            }

            var attributes = controllerActionDescriptor.EndpointMetadata.OfType<SwaggerGroupAttribute>().ToList();
            if (attributes.Any())
            {
                var groupNameAttribute = attributes.First();
                operation.Tags = new[] { new OpenApiTag { Name = groupNameAttribute.GroupName } };
            }
            else
            {
                operation.Tags = new[] { new OpenApiTag { Name = controllerActionDescriptor?.RouteValues["controller"] } };
            }
        }
    }
}
