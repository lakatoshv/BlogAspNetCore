using Blog.Contracts.V1.Responses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.CreateComment;

public record CreateCommentCommand(int PostId, string CommentBody, string UserId, string Name, string Email)
    : ICommand<CreatedResponse<int>>;
