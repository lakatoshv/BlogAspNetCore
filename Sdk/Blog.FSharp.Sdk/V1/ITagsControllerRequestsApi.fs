namespace Blog.FSharp.Sdk.V1

open System.Collections.Generic
open System.Threading.Tasks
open Refit
open Blog.FSharp.Contracts.V1.Responses
open Blog.FSharp.Contracts.V1.Requests.TagsRequests
open Blog.FSharp.Contracts.V1.Responses.TagsResponses

/// <summary>
/// Tags controller requests API interface.
/// </summary>
type ITagsControllerRequestsApi =

    /// <summary>
    /// Gets the tags.
    /// </summary>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the tags.</response>
    /// <response code="404">Unable to get the tags.</response>
    [<Get("/api/v1/tags/get-tags")>]
    abstract member GetTags :
        unit -> Task<List<TagResponse>>

    /// <summary>
    /// Gets the tags by filter.
    /// </summary>
    /// <param name="searchParameters">The search parameters.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the tags by filter.</response>
    /// <response code="404">Unable to get the tags by filter.</response>
    [<Post("/api/v1/tags/get-tags-by-filter")>]
    abstract member GetTagsByFilter :
        ?searchParameters:SearchParametersRequest -> Task<ApiResponse<PagedTagsResponse>>

    /// <summary>
    /// Gets the available tags.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the available tags.</response>
    /// <response code="404">Unable to get the available tags.</response>
    [<Get("/api/v1/tags/get-available-tags/{postId}")>]
    abstract member GetAvailableTags :
        postId:int -> Task<ApiResponse<List<TagResponse>>>

    /// <summary>
    /// Gets the tag.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the tag by id.</response>
    /// <response code="404">Unable to get the tag by id.</response>
    [<Get("/api/v1/tags/get-tag/{id}")>]
    abstract member GetTag :
        id:int -> Task<ApiResponse<TagResponse>>

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>Task.</returns>
    /// <response code="201">Creates the tag.</response>
    /// <response code="400">Unable to create the tag.</response>
    [<Post("/api/v1/tags/create")>]
    abstract member CreateAsync :
        model:CreateTagRequest -> Task<ApiResponse<TagResponse>>

    /// <summary>
    /// Edits the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="model">The model.</param>
    /// <returns>Task.</returns>
    /// <response code="204">Edits the tag.</response>
    /// <response code="400">Unable to edit the tag, model is invalid.</response>
    /// <response code="404">Unable to edit the tag, tag not found.</response>
    [<Put("/api/v1/tags/{id}")>]
    abstract member EditAsync :
        id:int * model:UpdateTagRequest -> Task<ApiResponse<TagResponse>>

    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Deletes the tag.</response>
    /// <response code="404">Unable to delete the tag, tag not found.</response>
    [<Delete("/api/v1/tags/{id}")>]
    abstract member DeleteAsync :
        id:int -> Task<ApiResponse<CreatedResponse<int>>>

