namespace BlogMinimalApi.Filters.SwaggerFilters;

using Blog.Core.Attributes.SwaggerAttributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

/// <summary>
/// Swagger operation filter for <see cref="SwaggerGroupAttribute"/>
/// </summary>
public class SwaggerGroupOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
            return;

        var attributes = controllerActionDescriptor.EndpointMetadata
            .OfType<SwaggerGroupAttribute>()
            .ToList();

        var tagName = attributes.Count != 0
            ? attributes.First().GroupName
            : controllerActionDescriptor.RouteValues["controller"];

        operation.Tags = new HashSet<OpenApiTagReference>
        {
            new (tagName)
        };
    }
}