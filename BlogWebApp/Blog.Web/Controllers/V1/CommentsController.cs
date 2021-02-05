using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Data.Models;
using Blog.Services.ControllerContext;
using Blog.Services.Core.Dtos;
using Blog.Services.Interfaces;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests;
using Blog.Contracts.V1.Requests.CommentsRequests;
using Blog.Contracts.V1.Responses;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Blog.Core.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.DateTime;

namespace Blog.Web.Controllers.V1
{
    /// <summary>
    /// Comments controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route(ApiRoutes.CommentsController.Comments)]
    [ApiController]
    [Produces(Consts.JsonType)]
    public class CommentsController : BaseController
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The comment service.
        /// </summary>
        private readonly ICommentsService _commentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsController"/> class.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="commentsService">The comments service.</param>
        /// <param name="mapper">The mapper.</param>
        public CommentsController(
            IControllerContext controllerContext,
            ICommentsService commentsService,
            IMapper mapper) : base(controllerContext)
        {
            _commentService = commentsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all comments.
        /// </summary>
        /// <returns>Task.</returns>
        /// <response code="200">Get all comments.</response>
        [ProducesResponseType(typeof(List<CommentResponse>), 200)]
        [HttpGet]
        public async Task<ActionResult> GetAllComments()
        {
            return Ok(_mapper.Map<List<CommentResponse>>(await _commentService.GetAllAsync().ConfigureAwait(false)));
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
        public async Task<ActionResult> GetComments([FromBody] SortParametersRequest sortParameters = null)
        {
            if (sortParameters is null)
            {
                sortParameters = new SortParametersRequest();
            }
            sortParameters.CurrentPage = sortParameters.CurrentPage ?? 1;
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
        [ProducesResponseType(typeof(CommentResponse), 200)]
        [ProducesResponseType(404)]
        [HttpPost(ApiRoutes.CommentsController.GetCommentsByPost)]
        public async Task<ActionResult> GetCommentsByPostAsync([FromRoute] int id, [FromBody] SortParametersRequest sortParameters = null)
        {
            if (sortParameters is null)
            {
                sortParameters = new SortParametersRequest();
            }
            sortParameters.CurrentPage = sortParameters.CurrentPage ?? 1;
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
        [HttpGet("{id}", Name = ApiRoutes.CommentsController.GetComment)]
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
            var baseUrl = $@"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" +
                              ApiRoutes.CommentsController.Comments + "/" +
                              ApiRoutes.CommentsController.GetComment + "/" + comment.Id.ToString();

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
        [HttpPut("{id}")]
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
        [HttpDelete("{id}")]
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
}