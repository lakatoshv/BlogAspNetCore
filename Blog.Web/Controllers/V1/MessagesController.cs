using System.Threading.Tasks;
using Blog.Data.Models;
using Blog.Services.ControllerContext;
using Blog.Services.Interfaces;
using Blog.Web.Contracts.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers.V1
{
    /// <summary>
    /// Messages controller.
    /// </summary>
    /// <seealso cref="BaseController" />
    [Route(ApiRoutes.MessagesController.Messages)]
    [ApiController]
    [AllowAnonymous]
    public class MessagesController : BaseController
    {
        /// <summary>
        /// The messages service.
        /// </summary>
        private readonly IMessagesService _messagesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesController"/> class.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="messagesService">The messages service.</param>
        public MessagesController(
            IControllerContext controllerContext,
            IMessagesService messagesService)
            : base(controllerContext)
        {
            _messagesService = messagesService;
        }

        // GET: Messages
        /// <summary>
        /// Async get filtered and sorted posts.
        /// </summary>
        /// <returns>Task.</returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var messages = await _messagesService.GetAllAsync(null).ConfigureAwait(false);

            if (messages == null)
            {
                return NotFound();
            }

            return Ok(messages);
        }

        /// <summary>
        /// Gets the recipient messages.
        /// </summary>
        /// <param name="recipientId">The recipient identifier.</param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.MessagesController.GetRecipientMessages)]
        public async Task<ActionResult> GetRecipientMessages(string recipientId)
        {
            var messages = await _messagesService
                .GetAllAsync(x => x.RecipientId.ToLower().Equals(recipientId.ToLower()))
                .ConfigureAwait(false);

            if (messages == null)
            {
                return NotFound();
            }

            return Ok(messages);
        }

        /// <summary>
        /// Gets the sender messages.
        /// </summary>
        /// <param name="senderEmail">The sender identifier.</param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.MessagesController.GetSenderMessages)]
        public async Task<ActionResult> GetSenderMessages(string senderEmail)
        {
            var messages = await _messagesService
                .GetAllAsync(x => x.SenderEmail.ToLower().Equals(senderEmail.ToLower()))
                .ConfigureAwait(false);

            if (messages == null)
            {
                return NotFound();
            }

            return Ok(messages);
        }

        /// <summary>
        /// Async get post by id.
        /// </summary>
        /// <param name="id">id.</param>
        /// <returns>Task</returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet(ApiRoutes.MessagesController.Show)]
        public async Task<ActionResult> Show(int id)
        {
            var message = await _messagesService.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        /// <summary>
        /// Async create new post.
        /// </summary>
        /// <param name="model">model.</param>
        /// <returns>IActionResult.</returns>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateAsync([FromBody] Message model)
        {
            if (CurrentUser != null)
            {
                model.SenderId = CurrentUser.Id;
            }
            await _messagesService.InsertAsync(model);

            return Ok();
        }

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
        public async Task<IActionResult> EditAsync(int id, [FromBody] Message model)
        {
            if (CurrentUser == null) return BadRequest(new { ErrorMessage = "Unauthorized" });
            if (!model.SenderId.Equals(CurrentUser.Id) || !model.RecipientId.Equals(CurrentUser.Id)) 
                return BadRequest(new { ErrorMessage = "You are not an author or recipient of the message." });

            //var message = await _messagesService.FindAsync(id);
            //var updatedModel = _mapper.Map(model, message);
            _messagesService.Update(model);

            var message = await _messagesService.FindAsync(id);
            //var mappedPost = _mapper.Map<Message>(postModel);

            return Ok(message);
        }

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
            if (CurrentUser == null) return BadRequest(new { ErrorMessage = "Unauthorized" });
            var message = await _messagesService.FindAsync(id);

            if (!message.SenderId.Equals(CurrentUser.Id) || !message.RecipientId.Equals(CurrentUser.Id))
            {
                return BadRequest(new { ErrorMessage = "You are not an author of the post." });
            }

            _messagesService.Delete(message);

            return Ok(new
            {
                Id = id
            });
        }
    }
}