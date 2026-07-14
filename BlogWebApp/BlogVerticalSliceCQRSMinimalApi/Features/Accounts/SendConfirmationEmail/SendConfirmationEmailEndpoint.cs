using Asp.Versioning;
using Blog.Contracts.V1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.SendConfirmationEmail;

public class SendConfirmationEmailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("Accounts")
            .MapToApiVersion(1.0)
            .WithTags("Accounts");

        group.MapGet(ApiRoutes.AccountsController.SendConfirmationEmail, async ([FromServices] HttpContext context, ISender sender) =>
            {
                var email = context.User.Identity?.Name;

                if (string.IsNullOrEmpty(email))
                    return Results.BadRequest();

                var result = await sender.Send(new SendConfirmationEmailQuery(Email: email));

                return Results.NoContent();
            })
            .WithName("SendConfirmationEmail")
            .Produces(StatusCodes.Status204NoContent)
            .WithSummary("Send Confirmation Email")
            .WithDescription("Send Confirmation Email");
    }
}