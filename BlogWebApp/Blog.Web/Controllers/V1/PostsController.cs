using System.Collections.Generic;
using System.Linq;
using Blog.Services.ControllerContext;
using Blog.Services.Core.Dtos.Posts;
using Blog.Web.Contracts.V1;
using Blog.Web.Contracts.V1.Requests.PostsRequests;
using Blog.Web.Contracts.V1.Responses;
using Blog.Web.Contracts.V1.Responses.PostsResponses;
using Blog.Web.Contracts.V1.Responses.UsersResponses;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Web.Controllers.V1
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Models;
    using Blog.Services.Core.Dtos;
    using Services.Interfaces;
    using VIewModels.Posts;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Posts controller.
    /// </summary>
    [Route(ApiRoutes.PostsController.Posts)]
    [ApiController]
    [AllowAnonymous]
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

        private readonly IPostsTagsRelationsService _postsTagsRelationsService;

        // private readonly ICommentService _commentsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsController"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="postsService">The posts service.</param>
        /// <param name="postsTagsRelationsService">The posts tags relations service.</param>
        /// <param name="mapper">The mapper.</param>
        public PostsController(
            IControllerContext controllerContext,
            IPostsService postsService,
            IPostsTagsRelationsService postsTagsRelationsService,

            // ICommentService commentService,
            IMapper mapper) : base(controllerContext)
        {
            _postsService = postsService;
            _postsTagsRelationsService = postsTagsRelationsService;

            // _commentsService = commentService;
            _mapper = mapper;
        }

        // GET: Posts
        /// <summary>
        /// Get all posts.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var posts = await _postsService.GetAllAsync().ConfigureAwait(false);
            if (posts == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PostResponse>(posts.ToList()));
        }

        // POST: Posts/get-posts
        /// <summary>
        /// Async get filtered and sorted posts.
        /// </summary>
        /// <param name="searchParameters">searchParameters.</param>
        /// <returns>Task.</returns>
        [HttpPost(ApiRoutes.PostsController.GetPosts)]
        public async Task<ActionResult> GetPosts([FromBody] PostsSearchParametersDto searchParameters)
        {
            if (searchParameters.SortParameters is null)
                searchParameters.SortParameters = new SortParametersDto();
            searchParameters.SortParameters.OrderBy = searchParameters.SortParameters.OrderBy ?? "asc";
            searchParameters.SortParameters.SortBy = searchParameters.SortParameters.SortBy ?? "Title";
            searchParameters.SortParameters.CurrentPage = searchParameters.SortParameters.CurrentPage ?? 1;
            searchParameters.SortParameters.PageSize = searchParameters.SortParameters.PageSize ?? 10;
            var posts = await _postsService.GetPostsAsync(searchParameters);

            if (posts == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PagedPostsResponse>(posts));
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
        [HttpPost(ApiRoutes.PostsController.UserPosts)]
        public async Task<ActionResult> GetUserPosts([FromRoute] string id, [FromBody] PostsSearchParametersDto searchParameters)
        {
            if (searchParameters.SortParameters is null)
                searchParameters.SortParameters = new SortParametersDto();
            searchParameters.SortParameters.OrderBy = searchParameters.SortParameters.OrderBy ?? "asc";
            searchParameters.SortParameters.SortBy = searchParameters.SortParameters.SortBy ?? "Title";
            searchParameters.SortParameters.CurrentPage = searchParameters.SortParameters.CurrentPage ?? 1;
            searchParameters.SortParameters.PageSize = 10;
            var posts = await _postsService.GetUserPostsAsync(id, searchParameters);

            if (posts == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PagedPostsResponse>(posts));
        }

        // GET: Posts/Show/5
        /// <summary>
        /// Async get post by id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Task</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet(ApiRoutes.PostsController.Show)]
        public async Task<ActionResult> Show([FromRoute] int id)
        {
            var sortParameters = new SortParametersDto
            {
                CurrentPage = 1,
                PageSize = 10
            };

            var post = await _postsService.GetPost(id, sortParameters);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PostWithPagedCommentsResponse>(post));
        }

        // POST: Posts
        /// <summary>
        /// Async create new post.
        /// </summary>
        /// <param name="model">model.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateAsync([FromBody] CreatePostRequest model)
        {
            if (CurrentUser == null)
            {
                return BadRequest(new {ErrorMessage = "Unauthorized"});
            }

            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.AuthorId))
            {
                return NotFound();
            }

            model.AuthorId = CurrentUser.Id;
            var postToCreate = _mapper.Map<Post>(model);
            var tags = _mapper.Map<List<Tag>>(model.Tags.Distinct());
            await _postsService.InsertAsync(postToCreate, tags);

            var response = new CreatedResponse<int> { Id = postToCreate.Id };

            var baseUrl = $@"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.PostsController.Show.Replace("{id}", postToCreate.Id.ToString());

            return Created(locationUrl, response);
        }

        [HttpPut(ApiRoutes.PostsController.LikePost)]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> LikePostAsync([FromRoute] int id)
        {
            if (CurrentUser == null)
            {
                return BadRequest(new {ErrorMessage = "Unauthorized"});
            }

            var model = await _postsService.GetPostAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            model.Likes++;
            _postsService.Update(model);

            var post = await _postsService.GetPostAsync(id);
            var mappedPost = _mapper.Map<PostViewResponse>(post);
            mappedPost.Author = _mapper.Map<ApplicationUserResponse>(post.Author);

            return Ok(mappedPost);
        }

        [HttpPut(ApiRoutes.PostsController.DislikePost)]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DislikePostAsync([FromRoute] int id)
        {
            if (CurrentUser == null)
            {
                return BadRequest(new {ErrorMessage = "Unauthorized"});
            }

            var model = await _postsService.GetPostAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            model.Dislikes++;
            _postsService.Update(model);

            var post = await _postsService.GetPostAsync(id);
            var mappedPost = _mapper.Map<PostViewResponse>(post);
            mappedPost.Author = _mapper.Map<ApplicationUserResponse>(post.Author);

            return Ok(mappedPost);
        }

        // PUT: Posts/5
        /// <summary>
        /// Async edit post by id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <param name="model">model.</param>
        /// <returns>Task.</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] UpdatePostRequest model)
        {
            if (CurrentUser == null)
            {
                return BadRequest(new {ErrorMessage = "Unauthorized"});
            }

            if (!model.AuthorId.Equals(CurrentUser.Id))
            {
                return BadRequest(new {ErrorMessage = "You are not an author of the post."});
            }


            var post = await _postsService.GetPostAsync(id);
            var updatedModel = _mapper.Map(model, post);
            // TODO Fix if possible
            updatedModel.Author = CurrentUser;
            // - - -
            _postsService.Update(updatedModel);
            var tags = _mapper.Map<List<Tag>>(model.Tags);
            await _postsTagsRelationsService.AddTagsToPost(post.Id, post.PostsTagsRelations.ToList(), tags);

            var postModel = await _postsService.GetPostAsync(id);
            var mappedPost = _mapper.Map<PostViewResponse>(postModel);

            return Ok(mappedPost);
        }

        // DELETE: Posts/5
        /// <summary>
        /// Async delete post by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync(int id, string authorId)
        {
            if (CurrentUser == null)
            {
                return BadRequest(new {ErrorMessage = "Unauthorized"});
            }

            var post = await _postsService.GetPostAsync(id);
            // var comments = await _commentsService.GetCommentsForPostAsync(id);
            // comments.ForEach(comment => _commentsService.Delete(comment));
            await _postsService.GetPostAsync(id);
            if (!post.AuthorId.Equals(CurrentUser.Id))
            {
                return BadRequest(new { ErrorMessage = "You are not an author of the post." });
            }

            _postsService.Delete(post);

            var response = new CreatedResponse<int> {Id = id};

            return Ok(response);
        }
    }
}