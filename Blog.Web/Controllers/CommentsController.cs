using System.Threading.Tasks;
using AutoMapper;
using Blog.Data.Models;
using Blog.Services.ControllerContext;
using Blog.Services.Core.Dtos;
using Blog.Services.Interfaces;
using Blog.Web.VIewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.DateTime;

namespace Blog.Web.Controllers
{
    /// <summary>
    /// Comments controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("api/[controller]")]
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

        // GET: Posts        
        /// <summary>
        /// Gets the comments by post asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="sortParameters">The sort parameters.</param>
        /// <returns>Task.</returns>
        [HttpPost("get-comments-by-post/{id}")]
        public async Task<ActionResult> GetCommentsByPostAsync(int id, [FromBody] SortParametersDto sortParameters)
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

        // POST: Comments/Create        
        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        [HttpPost("create")]
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

            return CreatedAtRoute("getComment", new { id = comment.Id }, comment);
        }
    }
}