using Blog.Contracts.V1.Requests.UsersRequests;
using Blog.Contracts.V1.Responses.UsersResponses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Sdk.V1
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Contracts.V1;
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
        [Get(ApiRoutes.AccountsController.Accounts + "/initialize")]
        Task<ApiResponse<List<RoleResponse>>> Initialize([FromRoute] string userId);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>Task.</returns>
        /// <response code="204">No content.</response>
        [Get(ApiRoutes.AccountsController.Accounts + "/" + ApiRoutes.AccountsController.GetAllUsers)]
        Task<ApiResponse<List<RoleResponse>>> GetAllUsers();

        /// <summary>
        /// Sends the verification email asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        [Get(ApiRoutes.AccountsController.Accounts + "/" + ApiRoutes.AccountsController.SendConfirmationEmail)]
        Task SendVerificationEmailAsync();

        /// <summary>
        /// Posts the asynchronous.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns>Task.</returns>
        /// <response code="200">User login.</response>
        /// <response code="400">Unable to user login.</response>
        [Post(ApiRoutes.AccountsController.Accounts + "/" + ApiRoutes.AccountsController.Login)]
        Task<ApiResponse<string>> PostAsync([FromBody] LoginRequest credentials);


        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        /// <response code="201">Register user.</response>
        /// <response code="400">Unable to register user.</response>
        [Post(ApiRoutes.AccountsController.Accounts + "/" + ApiRoutes.AccountsController.Register)]
        Task<ApiResponse<IdentityResult>> CreateAsync([FromBody] RegistrationRequest model);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        /// <response code="204">Change password.</response>
        /// <response code="400">Unable to change password.</response>
        [Put(ApiRoutes.AccountsController.Accounts + "/" + ApiRoutes.AccountsController.ChangePassword)]
        Task UpdateAsync([FromBody] ChangePasswordRequest model);
    }
}
