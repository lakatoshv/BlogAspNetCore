using Blog.Contracts.V1.Responses;
using Blog.Core.Enums;
using Blog.Data.Models;
using Blog.EntityServices.Interfaces;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.CreateMessage;

public class CreateMessageCommandHandler(IMessagesService service)
    : ICommandHandler<CreateMessageCommand, CreatedResponse<int>>
{
    public async Task<CreatedResponse<int>> Handle(CreateMessageCommand command, CancellationToken cancellationToken)
    {
        var message = new Message
        {
            SenderId = command.SenderId,
            RecipientId = command.RecipientId,
            SenderEmail = command.SenderEmail,
            SenderName = command.SenderName,
            Subject = command.Subject,
            Body = command.Body,
            MessageType = (MessageType)command.MessageType
        };

        await service.InsertAsync(message);

        return new CreatedResponse<int>
        {
            Id = message.Id
        };
    }
}