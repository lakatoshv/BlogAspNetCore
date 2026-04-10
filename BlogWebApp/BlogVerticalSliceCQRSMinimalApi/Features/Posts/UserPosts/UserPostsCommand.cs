using Blog.Contracts.V1.Responses.PostsResponses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.UserPosts;

public record UserPostsCommand(string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayType, string Tag, string Id)
    : ICommand<PagedPostsResponse>;
