using Blog.Contracts.V1.Responses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.CreateMessage;

public record CreateMessageCommand(string SenderId, string RecipientId, string SenderEmail, string SenderName,
    string Subject, string Body, int MessageType)
    : ICommand<CreatedResponse<int>>;