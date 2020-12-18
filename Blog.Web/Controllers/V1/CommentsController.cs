using System;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Data.Models;
using Blog.Services.ControllerContext;
using Blog.Services.Core.Dtos;
using Blog.Services.Interfaces;
using Blog.Web.Contracts.V1;
using Blog.Web.Contracts.V1.Responses;
using Blog.Web.VIewModels.Posts;
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
        public async Task<ActionResult> GetComments([FromBody] SortParametersDto sortParameters = null)
        {
            if (sortParameters is null)
            {
                sortParameters = new SortParametersDto();
            }

            sortParameters.CurrentPage = sortParameters.CurrentPage ?? 1;
            sortParameters.PageSize = 10;
            var comments = await _commentService.GetPagedComments(sortParameters);

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
        public async Task<ActionResult> GetCommentsByPostAsync([FromRoute] int id, [FromBody] SortParametersDto sortParameters = null)
        {
            if (sortParameters is null)
            {
                sortParameters = new SortParametersDto();
            }

            sortParameters.CurrentPage = sortParameters.CurrentPage ?? 1;
            sortParameters.PageSize = 10;
            var comments = await _commentService.GetPagedCommentsByPostId(id, sortParameters);

            if (comments == null)
            {
                return NotFound();
            }

            return Ok(comments);
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

            return Ok(comment);
        }

        // POST: Comments/Create        
        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        [HttpPost(ApiRoutes.CommentsController.CreateComment)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] CommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Bad(ModelState);
            }

            model.CreatedAt = Now;

            var comment = _mapper.Map<Comment>(model);

            await _commentService.InsertAsync(comment);

            return CreatedAtRoute(ApiRoutes.CommentsController.GetComment, new { id = comment.Id }, comment);
        }

        /// <summary>
        /// Edits the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize]
        public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] CommentViewModel model)
        {
            var originComment = await _commentService.GetCommentAsync(id);
            if (!originComment.UserId.Equals(model.UserId))
            {
                return NotFound();
            }
            model.CreatedAt = DateTime.Now;
            var updatedComment = _mapper.Map(model, originComment);
            _commentService.Update(updatedComment);

            var comment = await _commentService.GetCommentAsync(id);

            var mappedComment = _mapper.Map<CommentViewModel>(comment);
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

            _commentService.Delete(comment);

            return Ok(new
            {
                id = id
            });
        }
    }
}