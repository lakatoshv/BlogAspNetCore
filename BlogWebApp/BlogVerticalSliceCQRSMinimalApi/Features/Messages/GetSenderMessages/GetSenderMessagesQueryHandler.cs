using AutoMapper;
using Blog.Contracts.V1.Responses;
using Blog.Data.Specifications;
using Blog.EntityServices.Interfaces;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.GetSenderMessages;

public class GetSenderMessagesQueryHandler(IMessagesService service, IMapper mapper)
    : IQueryHandler<GetSenderMessagesQuery, List<MessageResponse>>
{
    public async Task<List<MessageResponse>> Handle(GetSenderMessagesQuery query, CancellationToken cancellationToken)
    {
        var messages = await service.GetAllAsync(
            new MessageSpecification(x =>
                x.SenderEmail.Equals(query.SenderEmail, StringComparison.CurrentCultureIgnoreCase)));

        return mapper.Map<List<MessageResponse>>(messages);
    }
}