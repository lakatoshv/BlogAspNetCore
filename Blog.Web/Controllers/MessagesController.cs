using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Services.ControllerContext;
using Blog.Services.Core.Dtos;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    /// <summary>
    /// Messages controller.
    /// </summary>
    /// <seealso cref="BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class MessagesController : BaseController
    {
        /// <summary>
        /// The messages service.
        /// </summary>
        private readonly IMessagesService _messagesService;

        // private readonly ICommentService _commentsService;

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
        [HttpGet("get-recipient-messages/{recipientId}")]
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
        [HttpGet("get-sender-messages/{senderEmail}")]
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
    }
}