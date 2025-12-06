namespace Blog.Web.Controllers.V1;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.DateTime;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests;
using Blog.Contracts.V1.Requests.CommentsRequests;
using Blog.Contracts.V1.Responses;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Blog.Contracts.V1.Responses.Chart;
using Data.Models;
using Blog.Services.Core.Dtos;
using EntityServices.ControllerContext;
using EntityServices.Interfaces;
using Core.Consts;
using Cache;

/// <summary>
/// Comments controller.
/// </summary>
/// <seealso cref="ControllerBase" />
/// <remarks>
/// Initializes a new instance of the <see cref="CommentsController"/> class.
/// </remarks>
/// <param name="controllerContext"></param>
/// <param name="commentsService">The comments service.</param>
/// <param name="mapper">The mapper.</param>
[Route(ApiRoutes.CommentsController.Comments)]
[ApiController]
[Produces(Consts.JsonType)]
public class CommentsController(
    IControllerContext controllerContext,
    ICommentsService commentsService,
    IMapper mapper)
    : BaseController(controllerContext)
{
    /// <summary>
    /// The mapper.
    /// </summary>
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// The comment service.
    /// </summary>
    private readonly ICommentsService _commentService = commentsService;

    /// <summary>
    /// Gets all comments.
    /// </summary>
    /// <returns>Task.</returns>
    /// <response code="200">Get all comments.</response>
    [ProducesResponseType(typeof(List<CommentResponse>), 200)]
    [HttpGet]
    [Cached(600)]
    public async Task<ActionResult> GetAllComments()
        => Ok(_mapper.Map<List<CommentResponse>>(await _commentService.GetAllAsync().ConfigureAwait(false)));

    // GET: Comments/comments-activity
    /// <summary>
    /// Async get comments activity.
    /// </summary>]
    /// <returns>Task.</returns>
    /// <response code="200">Get comments activity.</response>
    /// <response code="404">Unable to get comments activity.</response>
    [HttpGet(ApiRoutes.CommentsController.CommentsActivity)]
    [ProducesResponseType(typeof(ChartDataModel), 200)]
    [ProducesResponseType(404)]
    [Cached(600)]
    public async Task<ActionResult> CommentsActivity()
    {
        var commentsActivity = await _commentService.GetCommentsActivity().ConfigureAwait(false);
        if (commentsActivity == null)
        {
            return NotFound();
        }

        return Ok(commentsActivity);
    }

    /// <summary>
    /// Gets the comments.
    /// </summary>
    /// <param name="sortParameters">The sort parameters.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the comments.</response>
    /// <response code="404">Unable to gets the comments.</response>
    [ProducesResponseType(typeof(PagedCommentsResponse), 200)]
    [ProducesResponseType(404)]
    [HttpPost(ApiRoutes.CommentsController.GetCommentsByFilter)]
    [Cached(600)]
    public async Task<ActionResult> GetComments([FromBody] SortParametersRequest sortParameters = null)
    {
        sortParameters ??= new SortParametersRequest();

        sortParameters.CurrentPage ??= 1;
        sortParameters.PageSize = 10;

        var comments = await _commentService.GetPagedComments(_mapper.Map<SortParametersDto>(sortParameters));
        if (comments == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<PagedCommentsResponse>(comments));
    }

    // GET: Posts        
    /// <summary>
    /// Gets the comments by post asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="sortParameters">The sort parameters.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the comments by post asynchronous.</response>
    /// <response code="404">Unable to gets the comments by post.</response>
    [ProducesResponseType(typeof(List<CommentResponse>), 200)]
    [ProducesResponseType(404)]
    [HttpPost(ApiRoutes.CommentsController.GetCommentsByPost)]
    [Cached(600)]
    public async Task<ActionResult> GetCommentsByPostAsync([FromRoute] int id, [FromBody] SortParametersRequest sortParameters = null)
    {
        sortParameters ??= new SortParametersRequest();
        sortParameters.CurrentPage ??= 1;
        sortParameters.PageSize = 10;

        var comments = await _commentService.GetPagedCommentsByPostId(id, _mapper.Map<SortParametersDto>(sortParameters));
        if (comments == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<List<CommentResponse>>(comments));
    }

    /// <summary>
    /// Gets the comment.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the comment.</response>
    /// <response code="404">Unable to gets the comment.</response>
    [ProducesResponseType(typeof(CommentResponse), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id:int}", Name = ApiRoutes.CommentsController.GetComment)]
    // GET: Posts/Show/5
    public async Task<ActionResult> GetComment([FromRoute] int id)
    {
        var comment = await _commentService.GetCommentAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CommentResponse>(comment));
    }

    // POST: Comments/Create        
    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Task.</returns>
    /// <response code="201">Create the comment.</response>
    /// <response code="400">Unable to create the comment.</response>
    [HttpPost(ApiRoutes.CommentsController.CreateComment)]
    [ProducesResponseType(typeof(CommentResponse), 201)]
    [ProducesResponseType(typeof(ModelStateDictionary), 400)]
    [Authorize]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCommentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return Bad(ModelState);
        }

        var comment = _mapper.Map<Comment>(request);
        comment.CreatedAt = Now;
        await _commentService.InsertAsync(comment);
        var response = new CreatedResponse<int> {Id = comment.Id};
        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
        var locationUrl = baseUrl + "/" +
                          ApiRoutes.CommentsController.Comments + "/" +
                          ApiRoutes.CommentsController.GetComment + "/" + comment.Id;

        return CreatedAtRoute(locationUrl, response);
    }

    /// <summary>
    /// Edits the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="request"></param>
    /// <returns>Task.</returns>
    /// <response code="204">Update the comment.</response>
    /// <response code="400">Unable to update the comment, because model is invalid.</response>
    /// <response code="404">Unable to update the comment, because comment not found.</response>
    [HttpPut(ApiRoutes.CommentsController.EditComment)]
    [ProducesResponseType(typeof(CommentResponse), 204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [Authorize]
    public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] UpdateCommentRequest request)
    {
        var originComment = await _commentService.GetCommentAsync(id);
        if (!originComment.UserId.Equals(request.UserId))
        {
            return NotFound();
        }
        request.CreatedAt = DateTime.Now;
        var updatedComment = _mapper.Map(request, originComment);
        await _commentService.UpdateAsync(updatedComment).ConfigureAwait(false);

        var comment = await _commentService.GetCommentAsync(id);
        var mappedComment = _mapper.Map<CommentResponse>(comment);

        return Ok(mappedComment);
    }

    // POST: Comments/Delete/5
    /// <summary>
    /// Deletes the asynchronous.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Delete the comment.</response>
    /// <response code="404">Unable to delete the comment, because comment not found.</response>
    [HttpDelete(ApiRoutes.CommentsController.DeleteComment)]
    [ProducesResponseType(typeof(CreatedResponse<int>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var comment = await _commentService.GetCommentAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        await _commentService.DeleteAsync(comment);
        var response = new CreatedResponse<int> { Id = id };

        return Ok(response);
    }
}