using Blog.Contracts.V1.Responses.PostsResponses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.GetPostById;

public record GetPostByIdQuery(int Id) : IQuery<PostWithPagedCommentsResponse>;