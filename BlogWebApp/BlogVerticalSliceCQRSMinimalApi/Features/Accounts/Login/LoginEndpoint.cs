using Asp.Versioning;
using Blog.Contracts.V1;
using Carter;
using Mapster;
using MediatR;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.Login;

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup(ApiRoutes.AccountsController.Accounts)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithTags("Accounts");

        group.MapPost(ApiRoutes.AccountsController.Login,
                async (LoginRequest request, ISender sender) =>
                {
                    if (request is null)
                        return Results.BadRequest();

                    var command = request.Adapt<LoginCommand>();

                    var result = await sender.Send(command);

                    return result is null
                        ? Results.BadRequest("Invalid name or password")
                        : Results.Ok(result);
                })
            //.RequireAuthorization()
            .WithName("Login")
            .Produces(200)
            .Produces<string>(400)
            .WithSummary("Login")
            .WithDescription("Login")
            .HasApiVersion(1.0);
    }
}