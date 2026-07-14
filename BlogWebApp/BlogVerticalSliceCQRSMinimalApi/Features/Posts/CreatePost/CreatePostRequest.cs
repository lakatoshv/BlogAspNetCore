using Blog.Contracts.V1.Requests.TagsRequests;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.CreatePost;

public class GetPostsRequest(string Title, string Description, string Content, string ImageUrl,
    string AuthorId, DateTime CreatedAt, IList<TagRequest> Tags);