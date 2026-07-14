using AutoMapper;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Blog.EntityServices.Interfaces;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetAll;

public class GetAllQueryHandler(
    ICommentsService service,
    IMapper mapper)
    : IQueryHandler<GetAllQuery, List<CommentResponse>>
{
    public async Task<List<CommentResponse>> Handle(GetAllQuery query, CancellationToken cancellationToken)
    {
        var comments = await service.GetAllAsync();

        return mapper.Map<List<CommentResponse>>(comments);
    }
}