using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Data.Models;
using Blog.Services.ControllerContext;
using Blog.Services.Core.Dtos;
using Blog.Services.Interfaces;
using Blog.Web.Contracts.V1;
using Blog.Web.Contracts.V1.Requests;
using Blog.Web.Contracts.V1.Requests.CommentsRequests;
using Blog.Web.Contracts.V1.Responses;
using Blog.Web.Contracts.V1.Responses.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.DateTime;

namespace Blog.Web.Controllers.V1
{
    /// <summary>
    /// Comments controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route(ApiRoutes.CommentsController.Comments)]
    [ApiController]
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
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet]
        public async Task<ActionResult> GetAllComments()
        {
            return Ok(await _commentService.GetAllAsync().ConfigureAwait(false));
        }

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

            return Ok(comments);
        }

        // GET: Posts        
        /// <summary>
        /// Gets the comments by post asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="sortParameters">The sort parameters.</param>
        /// <returns>Task.</returns>
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

        [ProducesResponseType(200)]
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
        [HttpPost(ApiRoutes.CommentsController.CreateComment)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Bad(ModelState);
            }

            request.CreatedAt = Now;

            var comment = _mapper.Map<Comment>(request);
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
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
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
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
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