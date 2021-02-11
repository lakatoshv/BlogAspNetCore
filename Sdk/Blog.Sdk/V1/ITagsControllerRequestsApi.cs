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
        [Get(ApiRoutes.TagsController.Tags + "/" + ApiRoutes.TagsController.GetTags)]
        Task<ApiResponse<List<TagResponse>>> GetTags();

        /// <summary>
        /// Gets the tags by filter.
        /// </summary>
        /// <param name="searchParameters">The search parameters.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the tags by filter.</response>
        /// <response code="404">Unable to gets the tags by filter.</response>
        [Post(ApiRoutes.TagsController.Tags + "/" + ApiRoutes.TagsController.GetTagsByFilter)]
        Task<ApiResponse<PagedTagsResponse>> GetTagsByFilter([FromBody] SearchParametersRequest searchParameters = null);

        /// <summary>
        /// Gets the available tags.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the available tags.</response>
        /// <response code="404">Unable to gets the available tags.</response>
        [Get(ApiRoutes.TagsController.Tags + "/" + ApiRoutes.TagsController.GetAvailableTags)]
        Task<ApiResponse<List<TagResponse>>> GetAvailableTags([FromRoute] int postId);

        /// <summary>
        /// Gets the tag.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the tag by id.</response>
        /// <response code="404">Unable to gets the tag by id.</response>
        [Get(ApiRoutes.TagsController.Tags + "/" + ApiRoutes.TagsController.GetTag)]
        Task<ApiResponse<TagResponse>> GetTag([FromRoute] int id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        /// <response code="201">Create the tag.</response>
        /// <response code="400">Unable to create the tag.</response>
        [Post(ApiRoutes.TagsController.Tags + "/" + ApiRoutes.TagsController.CreateTag)]
        Task<ApiResponse<TagResponse>> CreateAsync([FromBody] CreateTagRequest model);

        /// <summary>
        /// Edits the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        /// <response code="204">Edits the tag.</response>
        /// <response code="400">Unable to edits the tag, model is invalid.</response>
        /// <response code="404">Unable to edits the tag, tag not found.</response>
        [Put(ApiRoutes.TagsController.Tags)]
        Task<ApiResponse<TagResponse>> EditAsync([FromRoute] int id, [FromBody] UpdateTagRequest model);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Deletes the tag.</response>
        /// <response code="404">Unable to deletes the tag, tag not found.</response>
        [Delete(ApiRoutes.TagsController.Tags)]
        Task<ApiResponse<CreatedResponse<int>>> DeleteAsync([FromRoute] int id);
    }
}