using Blog.Contracts.V1.Requests.TagsRequests;
using Blog.Contracts.V1.Responses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.CreatePost;

public record CreatePostCommand(string Title, string Description, string Content, string ImageUrl,
    string AuthorId, DateTime CreatedAt, IList<TagRequest> Tags)
    : ICommand<CreatedResponse<int>>;
