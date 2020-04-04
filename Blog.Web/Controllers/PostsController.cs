namespace Blog.Web.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Models;
    using Blog.Services.Core.Dtos;
    using Services.Interfaces;
    using Core.ControllerContext;
    using VIewModels.Posts;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Posts controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : BaseController
    {
        /// <summary>
        /// Mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Posts service.
        /// </summary>
        private readonly IPostsService _postsService;

        // private readonly ICommentService _commentsService;

        /// <summary>
        /// Applicant Controller Constructor
        /// </summary>
        /// <param name="controllerContext">controllerContext.</param>
        /// <param name="postsService">postService.</param>
        /// <param name="mapper">mapper.</param>
        public PostsController(
            IControllerContext controllerContext,
            IPostsService postsService,

            // ICommentService commentService,
            IMapper mapper) : base(controllerContext)
        {
            _postsService = postsService;

            // _commentsService = commentService;
            _mapper = mapper;
        }

        // GET: Posts
        /// <summary>
        /// Async get filtered and sorted posts.
        /// </summary>
        /// <param name="searchParameters">searchParameters.</param>
        /// <returns>Task.</returns>
        [HttpGet]
        public async Task<ActionResult> Index(SearchParametersDto searchParameters)
        {
            if (searchParameters.SortParameters is null)
                searchParameters.SortParameters = new SortParametersDto();
            searchParameters.SortParameters.OrderBy = searchParameters.SortParameters.OrderBy ?? "asc";
            searchParameters.SortParameters.SortBy = searchParameters.SortParameters.SortBy ?? "Title";
            searchParameters.SortParameters.CurrentPage = searchParameters.SortParameters.CurrentPage ?? 1;
            searchParameters.SortParameters.PageSize = 10;
            var posts = await _postsService.GetPostsAsync(searchParameters);

            // var postsModel = _mapper.Map<IList<PostViewModel>>(posts);
            /*var mappedPosts = _mapper.Map<PostViewModel>(posts);

            var model = new PostViewModel
            {
                Posts = mappedPosts,
                RecordsFiltered = jobs.Count
            };
            */

            if (posts == null)
            {
                return NotFound();
            }

            return Ok(posts);
        }


        // POST: Posts/get-posts
        /// <summary>
        /// Async get filtered and sorted posts.
        /// </summary>
        /// <param name="searchParameters">searchParameters.</param>
        /// <returns>Task.</returns>
        [HttpPost("get-posts")]
        public async Task<ActionResult> GetPosts([FromBody] SearchParametersDto searchParameters)
        {
            if (searchParameters.SortParameters is null)
                searchParameters.SortParameters = new SortParametersDto();
            searchParameters.SortParameters.OrderBy = searchParameters.SortParameters.OrderBy ?? "asc";
            searchParameters.SortParameters.SortBy = searchParameters.SortParameters.SortBy ?? "Title";
            searchParameters.SortParameters.CurrentPage = searchParameters.SortParameters.CurrentPage ?? 1;
            searchParameters.SortParameters.PageSize = 10;
            var posts = await _postsService.GetPostsAsync(searchParameters);

            // var postsModel = _mapper.Map<IList<PostViewModel>>(posts);
            /*var mappedPosts = _mapper.Map<PostViewModel>(posts);

            var model = new PostViewModel
            {
                Posts = mappedPosts,
                RecordsFiltered = jobs.Count
            };
            */

            if (posts == null)
            {
                return NotFound();
            }

            return Ok(posts);
        }


        // GET: Posts/user_posts/5
        /// <summary>
        /// Async get filtered and sorted user posts.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="searchParameters">searchParameters.</param>
        /// <returns>Task.</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpPost("user_posts/{id}")]
        public async Task<ActionResult> GetUserPosts(string id, [FromBody] SearchParametersDto searchParameters)
        {
            if (searchParameters.SortParameters is null)
                searchParameters.SortParameters = new SortParametersDto();
            searchParameters.SortParameters.OrderBy = searchParameters.SortParameters.OrderBy ?? "asc";
            searchParameters.SortParameters.SortBy = searchParameters.SortParameters.SortBy ?? "Title";
            searchParameters.SortParameters.CurrentPage = searchParameters.SortParameters.CurrentPage ?? 1;
            searchParameters.SortParameters.PageSize = 10;
            var post = await _postsService.GetUserPostsAsync(id, searchParameters);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // POST: Posts
        /// <summary>
        /// Async create new post.
        /// </summary>
        /// <param name="model">model.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult CreateAsync([FromBody] Post model)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(model.AuthorId))
            {
                _postsService.Insert(model);

                return Ok();
            }
            return NotFound();
        }
    }
}