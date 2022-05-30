namespace Blog.Sdk.V1
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blog.Contracts.V1;
    using Blog.Contracts.V1.Requests;
    using Blog.Contracts.V1.Requests.TagsRequests;
    using Blog.Contracts.V1.Responses;
    using Blog.Contracts.V1.Responses.TagsResponses;
    using Microsoft.AspNetCore.Mvc;
    using Refit;

    /// <summary>
    /// Tags controller requests api interface.
    /// </summary>
    public interface ITagsControllerRequestsApi
    {
        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the tags.</response>
        /// <response code="404">Unable to gets the tags.</response>
        [Get("/api/v1/tags/get-tags")]
        Task<List<TagResponse>> GetTags();

        /// <summary>
        /// Gets the tags by filter.
        /// </summary>
        /// <param name="searchParameters">The search parameters.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the tags by filter.</response>
        /// <response code="404">Unable to gets the tags by filter.</response>
        [Post("/api/v1/tags/get-tags-by-filter")]
        Task<ApiResponse<PagedTagsResponse>> GetTagsByFilter(SearchParametersRequest searchParameters = null);

        /// <summary>
        /// Gets the available tags.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the available tags.</response>
        /// <response code="404">Unable to gets the available tags.</response>
        [Get("/api/v1/tags/get-available-tags/{postId}")]
        Task<ApiResponse<List<TagResponse>>> GetAvailableTags(int postId);

        /// <summary>
        /// Gets the tag.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the tag by id.</response>
        /// <response code="404">Unable to gets the tag by id.</response>
        [Get("/api/v1/tags/get-tag/{id}")]
        Task<ApiResponse<TagResponse>> GetTag(int id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        /// <response code="201">Create the tag.</response>
        /// <response code="400">Unable to create the tag.</response>
        [Post("/api/v1/tags/create")]
        Task<ApiResponse<TagResponse>> CreateAsync(CreateTagRequest model);

        /// <summary>
        /// Edits the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        /// <response code="204">Edits the tag.</response>
        /// <response code="400">Unable to edits the tag, model is invalid.</response>
        /// <response code="404">Unable to edits the tag, tag not found.</response>
        [Put("/api/v1/tags/{id}")]
        Task<ApiResponse<TagResponse>> EditAsync(int id, UpdateTagRequest model);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Deletes the tag.</response>
        /// <response code="404">Unable to deletes the tag, tag not found.</response>
        [Delete("/api/v1/tags/{id}")]
        Task<ApiResponse<CreatedResponse<int>>> DeleteAsync(int id);
    }
}