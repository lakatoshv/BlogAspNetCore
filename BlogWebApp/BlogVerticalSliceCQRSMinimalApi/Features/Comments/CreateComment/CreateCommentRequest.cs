namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.CreateComment;

public record CreateCommentRequest(int PostId, string CommentBody, string UserId, string Name, string Email);
