// <copyright file="ValidateModelStateAttribute.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Attributes;

using System.Linq;
using Infrastructure.OperationResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// Custom validation handler for availability to whit OperationResult.
/// </summary>
public class ValidateModelStateAttribute : ActionFilterAttribute
{
    /// <inheritdoc cref="ActionNameAttribute"/>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

        var operation = OperationResult.CreateResult<object>();
        var messages = context.ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage));
        var message = string.Join(" ", messages);
        operation.AddError(message);
        context.Result = new OkObjectResult(operation);
    }
}