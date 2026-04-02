using AutoMapper;
using Blog.Contracts.V1.Responses.PostsResponses;
using Blog.EntityServices.Interfaces;
using Blog.Services.Core.Dtos;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.GetPostById;

public class GetPostByIdQueryHandler(IPostsService service, IMapper mapper)
    : IQueryHandler<GetPostByIdQuery, PostWithPagedCommentsResponse>
{
    public async Task<PostWithPagedCommentsResponse> Handle(GetPostByIdQuery query, CancellationToken cancellationToken)
    {
        var sort = new SortParametersDto
        {
            CurrentPage = 1,
            PageSize = 10
        };

        var post = await service.GetPost(query.Id, sort);

        return mapper.Map<PostWithPagedCommentsResponse>(post);
    }
}