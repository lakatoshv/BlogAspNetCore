using AutoMapper;
using Blog.Contracts.V1.Responses.PostsResponses;
using Blog.EntityServices.Interfaces;
using Blog.Services.Core.Dtos.Posts;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.UserPosts;

public class UserPostsCommandHandler(IPostsService service, IMapper mapper)
    : ICommandHandler<UserPostsCommand, PagedPostsResponse>
{
    public async Task<PagedPostsResponse> Handle(UserPostsCommand command, CancellationToken cancellationToken)
    {
        command = command with { OrderBy = command.OrderBy ?? "asc" };
        command = command with { SortBy = command.SortBy ?? "Title" };
        command = command with { CurrentPage = command.CurrentPage ?? 1 };
        command = command with { PageSize = command.PageSize ?? 10 };

        var posts = await service.GetUserPostsAsync(
            command.Id,
            mapper.Map<PostsSearchParametersDto>(command));

        return mapper.Map<PagedPostsResponse>(posts);
    }
}
