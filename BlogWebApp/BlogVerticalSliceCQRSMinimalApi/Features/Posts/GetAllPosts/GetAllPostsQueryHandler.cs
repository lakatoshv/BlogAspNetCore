using AutoMapper;
using Blog.Contracts.V1.Responses.PostsResponses;
using Blog.EntityServices.Interfaces;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.GetAllPosts;

public class GetAllPostsQueryHandler(IPostsService service, IMapper mapper)
    : IQueryHandler<GetAllPostsQuery, List<PostResponse>>
{
    public async Task<List<PostResponse>> Handle(GetAllPostsQuery query, CancellationToken cancellationToken)
    {
        var posts = await service.GetAllAsync();

        return mapper.Map<List<PostResponse>>(posts);
    }
}