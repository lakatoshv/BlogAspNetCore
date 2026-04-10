using AutoMapper;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Blog.EntityServices.Interfaces;
using Blog.Services.Core.Dtos;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetCommentsByPost;

public class GetCommentsByPostCommandHandler(ICommentsService service, IMapper mapper)
    : ICommandHandler<GetCommentsByPostCommand, PagedCommentsResponse>
{
    public async Task<PagedCommentsResponse> Handle(GetCommentsByPostCommand command, CancellationToken cancellationToken)
    {
        command = command with { CurrentPage = command.CurrentPage ?? 1 };
        command = command with { PageSize = 10 };

        var result = await service.GetPagedCommentsByPostId(command.PostId, 
            mapper.Map<SortParametersDto>(command));

        return mapper.Map<PagedCommentsResponse>(result);
    }
}
