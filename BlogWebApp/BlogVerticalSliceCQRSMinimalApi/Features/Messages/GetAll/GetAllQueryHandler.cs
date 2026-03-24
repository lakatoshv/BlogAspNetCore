using AutoMapper;
using Blog.Contracts.V1.Responses;
using Blog.EntityServices.Interfaces;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.GetAll;

public class GetAllQueryHandler(IMessagesService service, IMapper mapper)
    : IQueryHandler<GetAllQuery, List<MessageResponse>>
{
    public async Task<List<MessageResponse>> Handle(GetAllQuery query, CancellationToken cancellationToken)
    {
        var messages = await service.GetAllAsync();

        return mapper.Map<List<MessageResponse>>(messages);
    }
}