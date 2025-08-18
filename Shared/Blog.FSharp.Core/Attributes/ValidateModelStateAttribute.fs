namespace Blog.Core.Attributes

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc.Filters
open Blog.FSharp.Core.Infrastructure.OperationResults

/// <summary>
/// Custom validation handler for availability to work with OperationResult.
/// This attribute checks model state before the action executes.
/// </summary>
type ValidateModelStateAttribute() =
    inherit ActionFilterAttribute()

    /// <summary>
    /// Called before the action executes. If the model state is invalid,
    /// creates an OperationResult with error messages and returns it.
    /// </summary>
    /// <param name="context">The action executing context.</param>
    override _.OnActionExecuting(context: ActionExecutingContext) =
        if context.ModelState.IsValid then
            () // Do nothing if model state is valid
        else
            // Create an empty OperationResult<obj>
            let operation = OperationResult<obj>()

            // Collect all model state error messages
            let messages =
                context.ModelState.Values
                |> Seq.collect (fun x -> x.Errors |> Seq.map (fun e -> e.ErrorMessage))

            // Concatenate all messages into one string
            let message = String.concat " " messages

            // Add error metadata to the operation result
            OperationResultExtensions.addError operation message |> ignore

            // Set the action result to return the operation result as JSON
            context.Result <- OkObjectResult(operation) :> IActionResult
