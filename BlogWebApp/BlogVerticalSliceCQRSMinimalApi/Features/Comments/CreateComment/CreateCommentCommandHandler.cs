using Blog.Contracts.V1.Responses;
using Blog.Data.Models;
using Blog.EntityServices.Interfaces;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.CreateComment;

public class CreateCommentCommandHandler(ICommentsService service)
    : ICommandHandler<CreateCommentCommand, CreatedResponse<int>>
{
    public async Task<CreatedResponse<int>> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
    {
        var comment = new Comment
        {
            PostId = command.PostId,
            CommentBody = command.CommentBody,
            UserId = command.UserId,
            Name = command.Name,
            Email = command.Email,
            CreatedAt = DateTime.UtcNow
        };

        await service.InsertAsync(comment);

        return new CreatedResponse<int>
        {
            Id = comment.Id
        };
    }
}
