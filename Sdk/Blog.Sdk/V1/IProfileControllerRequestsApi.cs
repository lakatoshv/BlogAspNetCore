﻿using System.Threading.Tasks;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests.UsersRequests;
using Blog.Contracts.V1.Responses.UsersResponses;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Blog.Sdk.V1
{
    /// <summary>
    /// Profile controller requests api interface.
    /// </summary>
    public interface IProfileControllerRequestsApi
    {
        /// <summary>
        /// Shows the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the user profile.</response>
        /// <response code="404">Unable to gets the user profile.</response>
        [Get("/api/v1/profile/{id}")]
        Task<ApplicationUserResponse> Show(int id);

        /// <summary>
        /// Edits the asynchronous.
        /// </summary>
        /// <param name="profileId">The profile identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task</returns>
        /// <response code="204">Edits the user profile.</response>
        /// <response code="400">Unable to edits the user profile, model is invalid.</response>
        /// <response code="404">Unable to edits the user profile, profile not found.</response>
        [Put("/api/v1/profile/{profileId}")]
        Task<ApplicationUserResponse> EditAsync(int profileId, UpdateProfileRequest model);
    }
}