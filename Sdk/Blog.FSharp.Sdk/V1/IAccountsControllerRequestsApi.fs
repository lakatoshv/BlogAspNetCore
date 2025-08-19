namespace Blog.FSharp.Sdk.V1

open System.Collections.Generic
open System.Threading.Tasks
open Microsoft.AspNetCore.Identity
open Refit
open Blog.FSharp.Contracts.V1.Requests.UsersRequests
open Blog.FSharp.Contracts.V1.Responses.UsersResponses

/// <summary>
/// Defines the API endpoints for managing accounts via Refit.
/// </summary>
type IAccountsControllerRequestsApi =
    
    /// <summary>
    /// Initializes account data for the specified user by user identifier.
    /// </summary>
    /// <param name="userId">Unique identifier of the user.</param>
    /// <returns>A list of <see cref="RoleResponse"/> objects assigned to the user.</returns>
    /// <response code="200">Successfully retrieved user data.</response>
    /// <response code="400">Unable to retrieve user data.</response>
    [<Get("/api/v1/accounts/initialize/{userId}")>]
    abstract member Initialize : userId:string -> Task<List<RoleResponse>>

    /// <summary>
    /// Retrieves a list of all registered users.
    /// </summary>
    /// <returns>A list of <see cref="RoleResponse"/> objects representing users.</returns>
    /// <response code="204">No users found.</response>
    [<Get("/api/v1/accounts/get-all-users")>]
    abstract member GetAllUsers : unit -> Task<List<RoleResponse>>

    /// <summary>
    /// Sends a verification (confirmation) email to the currently authenticated user.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [<Get("/api/v1/accounts/send-confirmation-email")>]
    abstract member SendVerificationEmailAsync : unit -> Task

    /// <summary>
    /// Authenticates a user using provided login credentials.
    /// </summary>
    /// <param name="credentials">The login request containing email and password.</param>
    /// <returns>A JWT token string if login is successful.</returns>
    /// <response code="200">Successfully authenticated the user.</response>
    /// <response code="400">Invalid credentials or failed login attempt.</response>
    [<Post("/api/v1/accounts/login")>]
    abstract member PostAsync : credentials:LoginRequest -> Task<string>

    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="model">The registration request model containing user details.</param>
    /// <returns>An <see cref="IdentityResult"/> indicating the outcome of the registration.</returns>
    /// <response code="201">User successfully registered.</response>
    /// <response code="400">Registration failed due to invalid input or constraints.</response>
    [<Post("/api/v1/accounts/register")>]
    abstract member CreateAsync : model:RegistrationRequest -> Task<IdentityResult>

    /// <summary>
    /// Changes the password of an existing user.
    /// </summary>
    /// <param name="model">The change password request containing old and new passwords.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <response code="204">Password successfully changed.</response>
    /// <response code="400">Password change failed due to invalid input.</response>
    [<Put("/api/v1/accounts/change-password")>]
    abstract member UpdateAsync : model:ChangePasswordRequest -> Task

