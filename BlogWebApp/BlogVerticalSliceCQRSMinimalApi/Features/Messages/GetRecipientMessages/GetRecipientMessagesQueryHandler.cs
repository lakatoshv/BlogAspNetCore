using AutoMapper;
using Blog.Contracts.V1.Responses;
using Blog.Data.Specifications;
using Blog.EntityServices.Interfaces;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.GetRecipientMessages;

public class GetRecipientMessagesQueryHandler(IMessagesService service, IMapper mapper)
    : IQueryHandler<GetRecipientMessagesQuery, List<MessageResponse>>
{
    public async Task<List<MessageResponse>> Handle(GetRecipientMessagesQuery query,
        CancellationToken cancellationToken)
    {
        var messages = await service.GetAllAsync(
            new MessageSpecification(x =>
                x.RecipientId.Equals(query.RecipientId, StringComparison.CurrentCultureIgnoreCase)));

        return mapper.Map<List<MessageResponse>>(messages);
    }
}