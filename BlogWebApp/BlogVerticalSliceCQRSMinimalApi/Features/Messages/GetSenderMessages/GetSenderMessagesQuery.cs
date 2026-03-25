using Blog.Contracts.V1.Responses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.GetSenderMessages;

public record GetSenderMessagesQuery(string SenderEmail) : IQuery<List<MessageResponse>>;