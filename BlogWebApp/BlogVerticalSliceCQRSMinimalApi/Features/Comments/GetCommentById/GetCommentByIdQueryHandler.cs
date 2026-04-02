using AutoMapper;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Blog.EntityServices.Interfaces;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetCommentById;

public class GetCommentByIdQueryHandler(
    ICommentsService service,
    IMapper mapper)
    : IQueryHandler<GetCommentByIdQuery, CommentResponse>
{
    public async Task<CommentResponse> Handle(GetCommentByIdQuery query, CancellationToken cancellationToken)
    {
        var comment = await service.GetCommentAsync(query.Id);

        return mapper.Map<CommentResponse>(comment);
    }
}