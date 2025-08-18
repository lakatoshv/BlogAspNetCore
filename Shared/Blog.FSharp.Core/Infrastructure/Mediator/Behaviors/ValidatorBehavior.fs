namespace Blog.FSharp.Core.Infrastructure.Mediator.Behaviors

open System
open System.Collections.Generic
open System.Linq
open System.Threading
open System.Threading.Tasks
open FluentValidation
open MediatR
open Blog.FSharp.Core.Infrastructure.OperationResults
open Blog.FSharp.Core.Infrastructure.OperationResults.OperationResultExtensions
open Blog.FSharp.Core.Infrastructure

/// <summary>
/// Base validator behavior for requests.
/// </summary>
/// <typeparam name="TRequest">Request type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
type ValidatorBehavior<'TRequest,'TResponse when 'TResponse :> OperationResult>(validators: IEnumerable<IValidator<'TRequest>>) =
    interface IPipelineBehavior<'TRequest,'TResponse> with
        member _.Handle(request: 'TRequest, cancellationToken: CancellationToken, next: RequestHandlerDelegate<'TResponse>) : Task<'TResponse> =
            let failures =
                validators
                |> Seq.map (fun v -> v.Validate(ValidationContext<'TRequest>(request)))
                |> Seq.collect (fun r -> r.Errors)
                |> Seq.filter (fun e -> not (isNull e))
                |> Seq.toList

            if failures.Count() = 0 then
                // no failures, continue pipeline
                next.Invoke()
            else
                let tResponseType = typeof<'TResponse>
                if not (tResponseType.IsSubclassOf(typeof<OperationResult>)) then
                    raise (ValidationException(failures))
                
                let result = Activator.CreateInstance(tResponseType) :?> OperationResult
                OperationResultExtensions.addErrorEx result (new BlogException()) |> ignore

                failures |> List.iter (fun f ->
                    result.AppendLog($"{f.PropertyName}: {f.ErrorMessage}")
                )

                Task.FromResult(result :?> 'TResponse)
