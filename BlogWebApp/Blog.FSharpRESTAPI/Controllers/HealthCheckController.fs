namespace Blog.FSharpRESTAPI.Controllers

open System
open System.Linq
open System.Net
open System.Threading.Tasks

open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Diagnostics.HealthChecks

open Blog.Core.Consts
open Blog.Contracts.V1
open Blog.Contracts.V1.Responses.HealthChecks
open Blog.EntityServices.ControllerContext

[<Route(ApiRoutes.HealthController.Health)>]
[<ApiController>]
[<AllowAnonymous>]
[<Produces(Consts.JsonType)>]
type HealthCheckController(
    controllerContext : IControllerContext,
    healthCheckService : HealthCheckService
) =
    inherit BaseController(controllerContext)

    [<HttpGet>]
    [<ProducesResponseType(typeof<HealthCheckResponse>, 200)>]
    [<ProducesResponseType(typeof<HealthCheckResponse>, 400)>]
    [<ProducesResponseType(typeof<HealthCheckResponse>, 503)>]
    member this.Get() : Task<IActionResult> =
        task {

            try

                let! report =
                    healthCheckService.CheckHealthAsync()

                let response = HealthCheckResponse()

                response.Status <- report.Status.ToString()

                response.Checks <-
                    report.Entries
                        .Select(fun x ->
                            let check = HealthCheck()

                            check.Component <- x.Key
                            check.Status <- x.Value.Status.ToString()
                            check.Description <- x.Value.Description

                            check)

                response.Duration <- report.TotalDuration

                if report.Status = HealthStatus.Healthy then
                    return
                        (this.Ok(response)
                            :> IActionResult)

                else
                    return
                        (this.StatusCode(
                            int HttpStatusCode.ServiceUnavailable,
                            response)
                            :> IActionResult)

            with ex ->

                let message =
                    if isNull ex.InnerException then
                        ex.Message
                    else
                        ex.InnerException.Message

                return this.BadError(message)
        }