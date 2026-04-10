using Blog.Contracts.V1.Responses.PostsResponses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.GetPosts;

public record GetPostsCommand(string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayType, string Tag)
    : ICommand<PagedPostsResponse>;
