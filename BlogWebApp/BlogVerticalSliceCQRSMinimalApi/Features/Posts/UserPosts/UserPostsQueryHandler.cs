using AutoMapper;
using Blog.Contracts.V1.Responses.PostsResponses;
using Blog.EntityServices.Interfaces;
using Blog.Services.Core.Dtos.Posts;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.UserPosts;

public class UserPostsQueryHandler(IPostsService service, IMapper mapper)
    : IQueryHandler<UserPostsQuery, PagedPostsResponse>
{
    public async Task<PagedPostsResponse> Handle(UserPostsQuery query, CancellationToken cancellationToken)
    {
        query = query with { OrderBy = query.OrderBy ?? "asc" };
        query = query with { SortBy = query.SortBy ?? "Title" };
        query = query with { CurrentPage = query.CurrentPage ?? 1 };
        query = query with { PageSize = query.PageSize ?? 10 };

        var posts = await service.GetUserPostsAsync(
            query.Id,
            mapper.Map<PostsSearchParametersDto>(query));

        return mapper.Map<PagedPostsResponse>(posts);
    }
}