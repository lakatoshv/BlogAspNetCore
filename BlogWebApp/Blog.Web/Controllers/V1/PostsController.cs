using Blog.Web.Cache;

namespace Blog.Web.Controllers.V1
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Models;
    using Blog.Services.Core.Dtos;
    using Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Services.ControllerContext;
    using Blog.Services.Core.Dtos.Posts;
    using Blog.Contracts.V1;
    using Blog.Contracts.V1.Requests.PostsRequests;
    using Blog.Contracts.V1.Responses;
    using Blog.Contracts.V1.Responses.PostsResponses;
    using Blog.Contracts.V1.Responses.UsersResponses;
    using Core.Consts;
    using Microsoft.AspNetCore.Authorization;
    using Blog.Contracts.V1.Requests;

    /// <summary>
    /// Posts controller.
    /// </summary>
    [Route(ApiRoutes.PostsController.Posts)]
    [ApiController]
    [AllowAnonymous]
    [Produces(Consts.JsonType)]
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
        /// <returns>Task.</returns>
        /// <response code="200">Get all posts.</response>
        /// <response code="404">Unable to get all posts.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<PostResponse>), 200)]
        [ProducesResponseType(404)]
        [Cached(600)]
        public async Task<ActionResult> Index()
        {
            var posts = await _postsService.GetAllAsync().ConfigureAwait(false);
            if (posts == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<PostResponse>>(posts.ToList()));
        }

        // POST: Posts/get-posts
        /// <summary>
        /// Async get filtered and sorted posts.
        /// </summary>
        /// <param name="searchParameters">searchParameters.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Get filtered and sorted posts.</response>
        /// <response code="404">Unable to get filtered and sorted posts.</response>
        [HttpPost(ApiRoutes.PostsController.GetPosts)]
        [ProducesResponseType(typeof(PagedPostsResponse), 200)]
        [ProducesResponseType(404)]
        [Cached(600)]
        public async Task<ActionResult> GetPosts([FromBody] PostsSearchParametersRequest searchParameters)
        {
            if (searchParameters.SortParameters is null)
                searchParameters.SortParameters = new SortParametersRequest();
            searchParameters.SortParameters.OrderBy = searchParameters.SortParameters.OrderBy ?? "asc";
            searchParameters.SortParameters.SortBy = searchParameters.SortParameters.SortBy ?? "Title";
            searchParameters.SortParameters.CurrentPage = searchParameters.SortParameters.CurrentPage ?? 1;
            searchParameters.SortParameters.PageSize = searchParameters.SortParameters.PageSize ?? 10;
            var posts = await _postsService.GetPostsAsync(_mapper.Map<PostsSearchParametersDto>(searchParameters));

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
        /// <response code="200">Get filtered and sorted user posts.</response>
        /// <response code="404">Unable to get filtered and sorted user posts.</response>
        [ProducesResponseType(typeof(PagedPostsResponse), 200)]
        [ProducesResponseType(404)]
        [HttpPost(ApiRoutes.PostsController.UserPosts)]
        [Cached(600)]
        public async Task<ActionResult> GetUserPosts([FromRoute] string id, [FromBody] PostsSearchParametersRequest searchParameters)
        {
            if (searchParameters.SortParameters is null)
                searchParameters.SortParameters = new SortParametersRequest();
            searchParameters.SortParameters.OrderBy = searchParameters.SortParameters.OrderBy ?? "asc";
            searchParameters.SortParameters.SortBy = searchParameters.SortParameters.SortBy ?? "Title";
            searchParameters.SortParameters.CurrentPage = searchParameters.SortParameters.CurrentPage ?? 1;
            searchParameters.SortParameters.PageSize = 10;
            var posts = await _postsService.GetUserPostsAsync(id, _mapper.Map<PostsSearchParametersDto>(searchParameters));

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
        /// <response code="200">Get post by id.</response>
        /// <response code="404">Unable to get post by id.</response>
        [ProducesResponseType(typeof(PostWithPagedCommentsResponse), 200)]
        [ProducesResponseType(404)]
        [HttpGet(ApiRoutes.PostsController.Show)]
        [Cached(600)]
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
        /// <response code="201">Create new post.</response>
        /// <response code="400">Unable to create new post.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(PostResponse), 201)]
        [ProducesResponseType(typeof(object), 400)]
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

        /// <summary>
        /// Likes the post asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="204">Likes the post.</response>
        /// <response code="400">Unable to likes the post, model is invalid.</response>
        /// <response code="404">Unable to likes the post, post not found.</response>
        [HttpPut(ApiRoutes.PostsController.LikePost)]
        [Authorize]
        [ProducesResponseType(typeof(PostViewResponse), 204)]
        [ProducesResponseType(typeof(object), 400)]
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

        /// <summary>
        /// Dislikes the post asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="204">Dislikes the post.</response>
        /// <response code="400">Unable to dislikes the post, model is invalid.</response>
        /// <response code="404">Unable to dislikes the post, post not found.</response>
        [HttpPut(ApiRoutes.PostsController.DislikePost)]
        [Authorize]
        [ProducesResponseType(typeof(PostViewResponse), 204)]
        [ProducesResponseType(typeof(object), 400)]
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
        /// <response code="204">Edit post by id.</response>
        /// <response code="400">Unable to edit post by id, model is invalid.</response>
        /// <response code="404">Unable to edit post by id, post not found.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(PostViewResponse), 204)]
        [ProducesResponseType(typeof(object), 400)]
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
        /// <returns>Task.</returns>
        /// <response code="200">Delete post by id.</response>
        /// <response code="404">Unable to delete post by id, post not found.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(CreatedResponse<int>), 200)]
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