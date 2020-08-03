using System.Threading.Tasks;
using AutoMapper;
using Blog.Data.Models;
using Blog.Services.ControllerContext;
using Blog.Services.Core.Dtos.Posts;
using Blog.Services.Interfaces;
using Blog.Web.Contracts.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers.V1
{
    /// <summary>
    /// Tags controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route(ApiRoutes.TagsController.Tags)]
    [ApiController]
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
        /// <returns></returns>
        [HttpPost(ApiRoutes.TagsController.GetTags)]
        public async Task<ActionResult> GetTags()
        {
            var tags = await _tagsService.GetAllAsync();

            if (tags == null)
            {
                return NotFound();
            }

            return Ok(tags);
        }

        /// <summary>
        /// GetTag
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}", Name = ApiRoutes.TagsController.GetTag)]
        // GET: Posts/Show/5
        public async Task<ActionResult> GetTag(int id)
        {
            var tag = await _tagsService.FindAsync(id);
            if (tag == null)
                return NotFound();
            return Ok(tag);
        }

        [HttpPost(ApiRoutes.TagsController.CreateTag)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] Tag model)
        {
            if (!ModelState.IsValid)
            {
                return Bad(ModelState);
            }

            await _tagsService.InsertAsync(model);

            return CreatedAtRoute(ApiRoutes.TagsController.GetTag, new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize]
        public async Task<IActionResult> EditAsync(int id, [FromBody] Tag model)
        {
            var originTag = await _tagsService.FindAsync(id);
            var updatedTag = _mapper.Map(model, originTag);
            _tagsService.Update(updatedTag);

            var tag = await _tagsService.FindAsync(id);

            var mappedComment = _mapper.Map<TagViewDto>(tag);
            return Ok(mappedComment);
        }

        // POST: Tags/Delete/5
        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var comment = await _tagsService.FindAsync(id);

            _tagsService.Delete(comment);

            return Ok(new
            {
                id = id
            });
        }
    }
}