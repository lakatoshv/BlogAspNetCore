using Blog.Data.Models;
using Blog.Web.VIewModels.Posts;

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
            CreateMap<CommentViewModel, Comment>();
            CreateMap<Comment, PostViewModel>();
        }
    }
}