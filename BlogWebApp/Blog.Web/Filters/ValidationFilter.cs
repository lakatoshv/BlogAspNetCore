namespace Blog.Web.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Contracts.V1.Responses;

/// <summary>
/// Validation filter.
/// </summary>
/// <seealso cref="IAsyncActionFilter" />
public class ValidationFilter : IAsyncActionFilter
{
    /// <summary>
    /// Called asynchronously before the action, after model binding is complete.
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
    /// <param name="next">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate" />. Invoked to execute the next action filter or the action itself.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Before controller.
        if (!context.ModelState.IsValid)
        {
            var errorsInModelState = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(keyValueParam => keyValueParam.Key, keyValueParam => keyValueParam.Value.Errors.Select(x => x.ErrorMessage)).ToArray();

            var errorResponse = new ErrorResponse();

            foreach (var error in errorsInModelState)
            {
                foreach (var subError in error.Value)
                {
                    var errorModel = new ErrorModel
                    {
                        FieldName = error.Key,
                        Message = subError
                    };

                    errorResponse.Errors.Add(errorModel);
                }
            }

            context.Result = new BadRequestObjectResult(errorResponse);

            return;
        }

        await next();
        // After controller.
    }
}