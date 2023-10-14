namespace Blazor.Sdk.V1;

using System.Collections.Generic;
using System.Threading.Tasks;
using Blazor.Contracts.V1.Requests.UsersRequests;
using Blazor.Contracts.V1.Responses.UsersResponses;
using Microsoft.AspNetCore.Identity;
using Refit;

/// <summary>
/// Account controller requests api interface.
/// </summary>
public interface IAccountsControllerRequestsApi
{
    /// <summary>
    /// Initializes the specified user identifier.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Get user data by user id.</response>
    /// <response code="400">Unable to get user data by user id.</response>
    [Get("/api/v1/accounts/initialize/{userId}")]
    Task<List<RoleResponse>> Initialize(string userId);

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>Task.</returns>
    /// <response code="204">No content.</response>
    [Get("/api/v1/accounts/get-all-users")]
    Task<List<RoleResponse>> GetAllUsers();

    /// <summary>
    /// Sends the verification email asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    [Get("/api/v1/accounts/send-confirmation-email")]
    Task SendVerificationEmailAsync();

    /// <summary>
    /// Posts the asynchronous.
    /// </summary>
    /// <param name="credentials">The credentials.</param>
    /// <returns>Task.</returns>
    /// <response code="200">User login.</response>
    /// <response code="400">Unable to user login.</response>
    [Post("/api/v1/accounts/login")]
    Task<string> PostAsync(LoginRequest credentials);


    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Task.</returns>
    /// <response code="201">Register user.</response>
    /// <response code="400">Unable to register user.</response>
    [Post("/api/v1/accounts/register")]
    Task<IdentityResult> CreateAsync(RegistrationRequest model);

    /// <summary>
    /// Updates the asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Change password.</response>
    /// <response code="400">Unable to change password.</response>
    [Put("/api/v1/accounts/change-password")]
    Task UpdateAsync(ChangePasswordRequest model);
}