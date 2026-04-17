using Asp.Versioning;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Responses.UsersResponses;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.HealthCheck.Initialize;

public class InitializeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("HealthCheck")
                .MapToApiVersion(1.0)
                .WithTags("HealthCheck");

        group.MapGet("", async ([FromServices] ISender sender) =>
            {
                try
                {
                    var result = await sender.Send(new InitializeQuery());
                    /*
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

                    return report.Status == HealthStatus.Healthy
                        ? this.Ok(response)
                        : this.StatusCode((int)HttpStatusCode.ServiceUnavailable, response);*/

                    return Results.Ok();
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.InnerException);
                }
            })
            //.RequireAuthorization()
            .WithName("Initialize")
            .Produces<object>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Initialize")
            .WithDescription("Initialize");
    }
}