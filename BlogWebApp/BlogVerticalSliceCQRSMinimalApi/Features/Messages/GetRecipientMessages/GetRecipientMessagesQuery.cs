using Blog.Contracts.V1.Responses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.GetRecipientMessages;

public record GetRecipientMessagesQuery(string RecipientId) : IQuery<List<MessageResponse>>;