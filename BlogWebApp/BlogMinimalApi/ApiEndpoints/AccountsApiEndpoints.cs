using Asp.Versioning;
using AutoMapper;
using Blog.CommonServices.EmailServices.Interfaces;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests.UsersRequests;
using Blog.Contracts.V1.Responses.Chart;
using Blog.Contracts.V1.Responses.UsersResponses;
using Blog.Data.Models;
using Blog.EntityServices.EntityFrameworkServices.Identity.Auth;
using Blog.EntityServices.EntityFrameworkServices.Identity.Registration;
using Blog.EntityServices.EntityFrameworkServices.Identity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogMinimalApi.ApiEndpoints;

/// <summary>
/// Accounts Api endpoints.
/// </summary>
public class AccountsApiEndpoints : IRoutesInstaller
{
    public void InstallApiRoutes(WebApplication app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup(ApiRoutes.AccountsController.Accounts)
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .WithTags("Accounts");

        // -------------------------
        // PUBLIC ENDPOINTS
        // -------------------------

        var publicGroup = group.AllowAnonymous();

        // Initialize (roles list)
        publicGroup.MapGet(ApiRoutes.AccountsController.Initialize,
                async (string userId,
                    RoleManager<ApplicationRole> roleManager,
                    IMapper mapper) =>
                {
                    if (string.IsNullOrEmpty(userId))
                        return Results.BadRequest("Something went wrong");

                    var roles = await roleManager.Roles.ToListAsync();
                    var response = mapper.Map<List<RoleResponse>>(roles);

                    return Results.Ok(response);
                })
            .Produces<List<RoleResponse>>()
            .Produces<string>(400);


        // Get All Users
        publicGroup.MapGet(ApiRoutes.AccountsController.GetAllUsers,
                async (UserManager<ApplicationUser> userManager,
                    IMapper mapper) =>
                {
                    var users = await userManager.Users.Include(u => u.Roles).ToListAsync();
                    var response = mapper.Map<List<AccountResponse>>(users);

                    return Results.Ok(response);
                })
            .Produces<List<AccountResponse>>();


        // Login
        publicGroup.MapPost(ApiRoutes.AccountsController.Login,
                async (LoginRequest credentials,
                    IAuthService authService) =>
                {
                    if (credentials is null)
                        return Results.BadRequest();

                    var jwt = await authService.GetJwtAsync(credentials.Email, credentials.Password);

                    return jwt is null 
                        ? Results.BadRequest("Invalid name or password") 
                        : Results.Ok(jwt);
                })
            .Produces<string>()
            .Produces<string>(400);


        // Register
        publicGroup.MapPost(ApiRoutes.AccountsController.Register,
                async (RegistrationRequest model,
                    IMapper mapper,
                    IRegistrationService registrationService,
                    UserManager<ApplicationUser> userManager) =>
                {
                    if (model is null)
                        return Results.BadRequest();

                    model.UserName = model.Email;

                    var user = mapper.Map<ApplicationUser>(model);
                    user.CreatedOn = DateTime.UtcNow;
                    user.SecurityStamp = Guid.NewGuid().ToString();

                    var result = await registrationService.RegisterAsync(user, model.Password);

                    if (!result.Succeeded)
                        return Results.BadRequest(result);

                    model.Roles ??= ["User"];

                    foreach (var role in model.Roles)
                    {
                        var roleResult = await userManager.AddToRoleAsync(user, role);
                        if (!roleResult.Succeeded)
                            return Results.BadRequest(roleResult);
                    }

                    return Results.NoContent();
                })
            .Produces(204)
            .Produces<IdentityResult>(400);

        // -------------------------
        // PRIVATE ENDPOINTS
        // -------------------------

        var privateGroup = group.RequireAuthorization();

        // Send confirmation email
        privateGroup.MapGet(ApiRoutes.AccountsController.SendConfirmationEmail,
                async (IUserService userService,
                    IEmailExtensionService emailService,
                    HttpContext context) =>
                {
                    var email = context.User.Identity?.Name;

                    if (string.IsNullOrEmpty(email))
                        return Results.BadRequest();

                    var token = await userService.GetEmailVerificationTokenAsync(email);
                    await emailService.SendVerificationEmailAsync(email, token);

                    return Results.NoContent();
                })
            .Produces(204);

        // Users activity
        privateGroup.MapGet(ApiRoutes.AccountsController.UsersActivity,
                async (IUserService userService) =>
                {
                    var activity = await userService.GetUsersActivity();

                    return activity is null
                        ? Results.NotFound()
                        : Results.Ok(activity);
                })
            .Produces<ChartDataModel>()
            .Produces(404);

        // Change password
        privateGroup.MapPut(ApiRoutes.AccountsController.ChangePassword,
                async (ChangePasswordRequest model,
                    IUserService userService,
                    HttpContext context) =>
                {
                    if (model is null)
                        return Results.BadRequest();

                    var username = context.User.Identity?.Name;

                    if (string.IsNullOrEmpty(username))
                        return Results.BadRequest();

                    var result = await userService.ChangePasswordAsync(
                        username,
                        model.OldPassword,
                        model.NewPassword);

                    return result.Succeeded
                        ? Results.NoContent()
                        : Results.BadRequest(result.Errors);
                })
            .Produces(204)
            .Produces(400);
    }
}