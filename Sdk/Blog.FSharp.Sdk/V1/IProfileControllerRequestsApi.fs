namespace Blog.FSharp.Sdk.V1

open System.Threading.Tasks
open Refit
open Blog.FSharp.Contracts.V1.Requests.UsersRequests
open Blog.FSharp.Contracts.V1.Responses.UsersResponses

/// <summary>
/// Profile controller requests API interface.
/// </summary>
type IProfileControllerRequestsApi =

    /// <summary>
    /// Shows the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the user profile.</response>
    /// <response code="404">Unable to get the user profile.</response>
    [<Get("/api/v1/profile/{id}")>]
    abstract member Show :
        id:int -> Task<ApplicationUserResponse>

    /// <summary>
    /// Edits the asynchronous.
    /// </summary>
    /// <param name="profileId">The profile identifier.</param>
    /// <param name="model">The model.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Edits the user profile.</response>
    /// <response code="400">Unable to edit the user profile, model is invalid.</response>
    /// <response code="404">Unable to edit the user profile, profile not found.</response>
    [<Put("/api/v1/profile/{profileId}")>]
    abstract member EditAsync :
        profileId:int * model:UpdateProfileRequest -> Task<ApplicationUserResponse>

