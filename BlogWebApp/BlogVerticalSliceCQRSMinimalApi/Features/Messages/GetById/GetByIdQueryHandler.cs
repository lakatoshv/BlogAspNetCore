using AutoMapper;
using Blog.Contracts.V1.Responses;
using Blog.EntityServices.Interfaces;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Messages.GetById;

public class GetByIdQueryHandler(IMessagesService service, IMapper mapper)
    : IQueryHandler<GetByIdQuery, MessageResponse>
{
    public async Task<MessageResponse> Handle(GetByIdQuery query, CancellationToken cancellationToken)
        => mapper.Map<MessageResponse>(await service.FindAsync(query.Id));
}