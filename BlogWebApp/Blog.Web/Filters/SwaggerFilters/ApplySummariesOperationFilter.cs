namespace Blog.Web.Filters.SwaggerFilters;

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// Swagger Method Info Generator from summary for <see cref="M:GetPaged{T}"/>
/// </summary>
public class ApplySummariesOperationFilter : IOperationFilter
{
    /// <inheritdoc />
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
        {
            return;
        }

        var actionName = controllerActionDescriptor.ActionName;
        if (actionName != "GetPaged")
        {
            return;
        }

        var resourceName = controllerActionDescriptor.ControllerName;
        operation.Summary = $"Returns paged list of the {resourceName} as IPagedList wrapped with OperationResult";
    }
}