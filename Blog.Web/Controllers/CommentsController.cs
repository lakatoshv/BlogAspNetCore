using System.Threading.Tasks;
using AutoMapper;
using Blog.Services.ControllerContext;
using Blog.Services.Core.Dtos;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("get-comments-by-post/{id}")]
        // GET: Posts
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
    }
}