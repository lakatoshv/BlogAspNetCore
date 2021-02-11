namespace Blog.Web.Controllers.V1
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Blog.Data.Models;
    using Blog.Services.ControllerContext;
    using Blog.Services.Core.Dtos;
    using Blog.Services.Interfaces;
    using Blog.Contracts.V1;
    using Blog.Contracts.V1.Requests;
    using Blog.Contracts.V1.Requests.TagsRequests;
    using Blog.Contracts.V1.Responses;
    using Blog.Contracts.V1.Responses.TagsResponses;
    using Blog.Core.Consts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    /// <summary>
    /// Tags controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route(ApiRoutes.TagsController.Tags)]
    [ApiController]
    [Produces(Consts.JsonType)]
    public class TagsController : BaseController
    {
        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The tags service.
        /// </summary>
        private readonly ITagsService _tagsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsController"/> class.
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="tagsService"></param>
        /// <param name="mapper">The mapper.</param>
        public TagsController(
            IControllerContext controllerContext,
            ITagsService tagsService,
            IMapper mapper) : base(controllerContext)
        {
            _tagsService = tagsService;
            _mapper = mapper;
        }

        // GET: Tags               
        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the tags.</response>
        /// <response code="404">Unable to gets the tags.</response>
        [HttpGet(ApiRoutes.TagsController.GetTags)]
        [ProducesResponseType(typeof(List<TagResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetTags()
        {
            var tags = await _tagsService.GetAllAsync();
            if (tags == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<TagResponse>>(tags));
        }

        /// <summary>
        /// Gets the tags by filter.
        /// </summary>
        /// <param name="searchParameters">The search parameters.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the tags by filter.</response>
        /// <response code="404">Unable to gets the tags by filter.</response>
        [HttpPost(ApiRoutes.TagsController.GetTagsByFilter)]
        [ProducesResponseType(typeof(PagedTagsResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetTagsByFilter([FromBody] SearchParametersRequest searchParameters = null)
        {
            if (searchParameters.SortParameters is null)
                searchParameters.SortParameters = new SortParametersRequest();

            searchParameters.SortParameters.OrderBy = searchParameters.SortParameters.OrderBy ?? "asc";
            searchParameters.SortParameters.SortBy = searchParameters.SortParameters.SortBy ?? "Title";
            searchParameters.SortParameters.CurrentPage = searchParameters.SortParameters.CurrentPage ?? 1;
            searchParameters.SortParameters.PageSize = searchParameters.SortParameters.PageSize ?? 10;
            var tags = await _tagsService.GetTagsAsync(_mapper.Map<SearchParametersDto>(searchParameters));
            if (tags == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PagedTagsResponse>(tags));
        }

        /// <summary>
        /// Gets the available tags.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the available tags.</response>
        /// <response code="404">Unable to gets the available tags.</response>
        [HttpGet(ApiRoutes.TagsController.GetAvailableTags)]
        [ProducesResponseType(typeof(List<TagResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetAvailableTags([FromRoute] int postId)
        {
            var tags = await _tagsService.GetAllAsync(x => x.PostsTagsRelations == null || x.PostsTagsRelations.Any(y => y.PostId != postId));
            if (tags == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<TagResponse>>(tags));
        }

        /// <summary>
        /// GetTag
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task.</returns>
        /// <response code="200">Gets the tag by id.</response>
        /// <response code="404">Unable to gets the tag by id.</response>
        [ProducesResponseType(typeof(TagResponse), 200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}", Name = ApiRoutes.TagsController.GetTag)]
        // GET: Posts/Show/5
        public async Task<ActionResult> GetTag([FromRoute] int id)
        {
            var tag = await _tagsService.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TagResponse>(tag));
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        /// <response code="201">Create the tag.</response>
        /// <response code="400">Unable to create the tag.</response>
        [HttpPost(ApiRoutes.TagsController.CreateTag)]
        [ProducesResponseType(typeof(TagResponse), 201)]
        [ProducesResponseType(typeof(ModelStateDictionary), 400)]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTagRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Bad(ModelState);
            }

            if (await _tagsService.AnyAsync(x => x.Title.ToLower().Equals(model.Title.ToLower())))
            {
                return Bad(ModelState);
            }

            var tag = _mapper.Map<Tag>(model);
            await _tagsService.InsertAsync(tag);

            var response = new CreatedResponse<int> { Id = tag.Id };

            var baseUrl = $@"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.TagsController.GetTag.Replace("{id}", tag.Id.ToString());

            return Created(locationUrl, response);
        }

        /// <summary>
        /// Edits the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns>Task.</returns>
        /// <response code="204">Edits the tag.</response>
        /// <response code="400">Unable to edits the tag, model is invalid.</response>
        /// <response code="404">Unable to edits the tag, tag not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TagResponse), 204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize]
        public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] UpdateTagRequest model)
        {
            var originTag = await _tagsService.FindAsync(id);
            if (originTag == null)
            {
                return NotFound();
            }

            var updatedTag = _mapper.Map(model, originTag);
            _tagsService.Update(updatedTag);

            var tag = await _tagsService.FindAsync(id);
            var mappedTag = _mapper.Map<TagResponse>(tag);

            return Ok(mappedTag);
        }

        // POST: Tags/Delete/5
        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        /// <response code="200">Deletes the tag.</response>
        /// <response code="404">Unable to deletes the tag, tag not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CreatedResponse<int>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var comment = await _tagsService.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _tagsService.Delete(comment);
            var response = new CreatedResponse<int> {Id = id};

            return Ok(response);
        }
    }
}