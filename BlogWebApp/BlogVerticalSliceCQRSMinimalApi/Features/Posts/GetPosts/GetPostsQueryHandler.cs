using AutoMapper;
using Blog.Contracts.V1.Responses.PostsResponses;
using Blog.EntityServices.Interfaces;
using Blog.Services.Core.Dtos.Posts;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.GetPosts;

public class GetPostsQueryHandler(IPostsService service, IMapper mapper)
    : IQueryHandler<GetPostsQuery, PagedPostsResponse>
{
    public async Task<PagedPostsResponse> Handle(GetPostsQuery query, CancellationToken cancellationToken)
    {
        query = query with { OrderBy = query.OrderBy ?? "asc" };
        query = query with { SortBy = query.SortBy ?? "Title" };
        query = query with { CurrentPage = query.CurrentPage ?? 1 };
        query = query with { PageSize = query.PageSize ?? 10 };

        var posts = await service.GetPostsAsync(
            mapper.Map<PostsSearchParametersDto>(query));

        return mapper.Map<PagedPostsResponse>(posts);
    }
}