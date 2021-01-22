using Blog.Data.Models;
using Blog.Web.Contracts.V1.Requests.CommentsRequests;
using Blog.Web.Contracts.V1.Responses.Posts;

namespace Blog.Web.Mappers.Posts
{
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
        }
    }
}