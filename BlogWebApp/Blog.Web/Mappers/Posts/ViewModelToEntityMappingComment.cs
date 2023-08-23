namespace Blog.Web.Mappers.Posts;

using Data.Models;
using Blog.Services.Core.Dtos.Posts;
using Contracts.V1.Requests.CommentsRequests;
using Contracts.V1.Responses.CommentsResponses;

/// <summary>
/// View model to entity mapping comment.
/// </summary>
/// <seealso cref="AutoMapper.Profile" />
public class ViewModelToEntityMappingComment : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelToEntityMappingComment"/> class.
    /// </summary>
    public ViewModelToEntityMappingComment()
    {
        CreateMap<CreateCommentRequest, Comment>();
        CreateMap<UpdateCommentRequest, Comment>();
        CreateMap<Comment, CommentResponse>();
        CreateMap<CommentsViewDto, PagedCommentsResponse>();
    }
}