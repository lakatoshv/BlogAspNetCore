﻿namespace Blog.Web.Controllers.V1;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Data.Models;
using Services.ControllerContext;
using Services.Interfaces;
using Blog.Contracts.V1;
using Blog.Contracts.V1.Requests.MessagesRequests;
using Blog.Contracts.V1.Responses;
using Core.Consts;
using Data.Specifications;
using Cache;

/// <summary>
/// Messages controller.
/// </summary>
/// <seealso cref="BaseController" />
[Route(ApiRoutes.MessagesController.Messages)]
[ApiController]
[AllowAnonymous]
[Produces(Consts.JsonType)]
public class MessagesController : BaseController
{
    /// <summary>
    /// The messages service.
    /// </summary>
    private readonly IMessagesService _messagesService;

    /// <summary>
    /// The mapper.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessagesController"/> class.
    /// </summary>
    /// <param name="controllerContext">The controller context.</param>
    /// <param name="messagesService">The messages service.</param>
    /// <param name="mapper"></param>
    public MessagesController(
        IControllerContext controllerContext,
        IMessagesService messagesService,
        IMapper mapper)
        : base(controllerContext)
    {
        _messagesService = messagesService;
        _mapper = mapper;
    }

    // GET: Messages        
    /// <summary>
    /// Get all messages.
    /// </summary>
    /// <returns>Task.</returns>
    /// <response code="200">Get all messages.</response>
    /// <response code="404">Unable to get all messages.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<MessageResponse>), 200)]
    [ProducesResponseType(404)]
    [Cached(600)]
    public async Task<ActionResult> Index()
    {
        var messages = await _messagesService.GetAllAsync().ConfigureAwait(false);

        if (messages == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<List<MessageResponse>>(messages.ToList()));
    }

    /// <summary>
    /// Gets the recipient messages.
    /// </summary>
    /// <param name="recipientId">The recipient identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the recipient messages.</response>
    /// <response code="404">Unable to gets the recipient messages.</response>
    [HttpGet(ApiRoutes.MessagesController.GetRecipientMessages)]
    [ProducesResponseType(typeof(List<MessageResponse>), 200)]
    [ProducesResponseType(404)]
    [Cached(600)]
    public async Task<ActionResult> GetRecipientMessages([FromRoute] string recipientId)
    {
        var messages = await _messagesService
            .GetAllAsync(new MessageSpecification(x => x.RecipientId.ToLower().Equals(recipientId.ToLower())))
            .ConfigureAwait(false);

        if (messages == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<List<MessageResponse>>(messages.ToList()));
    }

    /// <summary>
    /// Gets the sender messages.
    /// </summary>
    /// <param name="senderEmail">The sender identifier.</param>
    /// <returns>Task.</returns>
    /// <response code="200">Gets the sender messages.</response>
    /// <response code="404">Unable to gets the sender messages.</response>
    [HttpGet(ApiRoutes.MessagesController.GetSenderMessages)]
    [ProducesResponseType(typeof(List<MessageResponse>), 200)]
    [ProducesResponseType(404)]
    [Cached(600)]
    public async Task<ActionResult> GetSenderMessages([FromRoute] string senderEmail)
    {
        var messages = await _messagesService
            .GetAllAsync(new MessageSpecification(x => x.SenderEmail.ToLower().Equals(senderEmail.ToLower())))
            .ConfigureAwait(false);

        if (messages == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<List<MessageResponse>>(messages.ToList()));
    }

    /// <summary>
    /// Async get message by id.
    /// </summary>
    /// <param name="id">id.</param>
    /// <returns>Task</returns>
    /// <response code="200">Gets the messages by id.</response>
    /// <response code="404">Unable to gets the messages by id.</response>
    [ProducesResponseType(typeof(MessageResponse), 200)]
    [ProducesResponseType(404)]
    [HttpGet(ApiRoutes.MessagesController.Show)]
    public async Task<ActionResult> Show([FromRoute] int id)
    {
        var message = await _messagesService.FindAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<MessageResponse>(message));
    }

    /// <summary>
    /// Creates the asynchronous.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Task</returns>
    /// <response code="201">Creates the message.</response>
    /// <response code="400">Unable to create the message.</response>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateMessageRequest request)
    {
        if (CurrentUser != null)
        {
            request.SenderId = CurrentUser.Id;
        }

        await _messagesService.InsertAsync(_mapper.Map<Message>(request));

        return Ok();
    }

    /// <summary>
    /// Async edit post by id.
    /// </summary>
    /// <param name="id">id.</param>
    /// <param name="request"></param>
    /// <returns>Task.</returns>
    /// <response code="204">Edit the message.</response>
    /// <response code="400">Unable to edit the message, model is invalid.</response>
    /// <response code="404">Unable to edit the message, message not found.</response>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(MessageResponse), 204)]
    [ProducesResponseType(typeof(object), 400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] UpdateMessageRequest request)
    {
        if (CurrentUser == null)
        {
            return BadRequest(new {ErrorMessage = "Unauthorized"});
        }

        if (!request.SenderId.Equals(CurrentUser.Id) || !request.RecipientId.Equals(CurrentUser.Id))
        {
            return BadRequest(new {ErrorMessage = "You are not an author or recipient of the message."});
        }

        //var message = await _messagesService.FindAsync(id);
        //var updatedModel = _mapper.Map(model, message);
        await _messagesService.UpdateAsync(_mapper.Map<Message>(request));

        var message = await _messagesService.FindAsync(id);

        return Ok(_mapper.Map<MessageResponse>(message));
    }

    /// <summary>
    /// Async delete post by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="authorId"></param>
    /// <returns>Task.</returns>
    /// <response code="200">Delete the message.</response>
    /// <response code="404">Unable to delete the message, message not found.</response>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(CreatedResponse<int>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromBody] string authorId)
    {
        if (CurrentUser == null)
        {
            return BadRequest(new {ErrorMessage = "Unauthorized"});
        }

        var message = await _messagesService.FindAsync(id);

        if (!message.SenderId.Equals(CurrentUser.Id) || !message.RecipientId.Equals(CurrentUser.Id))
        {
            return BadRequest(new { ErrorMessage = "You are not an author of the post." });
        }

        await _messagesService.DeleteAsync(message);
        var response = new CreatedResponse<int> { Id = id };

        return Ok(response);
    }
}