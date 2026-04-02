using AutoMapper;
using Blog.Contracts.V1.Requests.UsersRequests;
using Blog.Data.Models;
using Blog.EntityServices.EntityFrameworkServices.Identity.Registration;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;
using Microsoft.AspNetCore.Identity;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.Register;

public class RegisterCommandHandler(IMapper mapper,
    IRegistrationService registrationService,
    UserManager<ApplicationUser> userManager) : ICommandHandler<RegisterCommand, IdentityResult>
{
    public async Task<IdentityResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var model = new RegistrationRequest
        {
            Email = command.Email,
            Password = command.Password,
            ConfirmPassword = command.ConfirmPassword,
            FirstName = command.FirstName,
            LastName = command.LastName,
            UserName = command.Email,
            PhoneNumber = command.PhoneNumber,
            ConcurrencyStamp = command.ConcurrencyStamp,
            Roles = command.Roles
        };

        var user = mapper.Map<ApplicationUser>(model);
        user.CreatedOn = DateTime.UtcNow;
        user.SecurityStamp = Guid.NewGuid().ToString();

        var result = await registrationService.RegisterAsync(user, model.Password);

        //if (!result.Succeeded)
            //return Results.BadRequest(result);

        model.Roles ??= ["User"];

        foreach (var role in model.Roles)
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);
            //if (!roleResult.Succeeded)
                //return Results.BadRequest(roleResult);
        }

        return result;
    }
}
