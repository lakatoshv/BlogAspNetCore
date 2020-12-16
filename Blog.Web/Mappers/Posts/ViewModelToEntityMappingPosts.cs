using Blog.Data.Models;
using Blog.Services.Core.Dtos.Posts;
using Blog.Web.VIewModels.Posts;

namespace Blog.Web.Mappers.Posts
{
    /// <summary>
    /// View model to entity mapping posts.
    /// </summary>
    public class ViewModelToEntityMappingPosts : AutoMapper.Profile
    {
        /// <summary>
        /// Post maps.
        /// </summary>
        public ViewModelToEntityMappingPosts()
        {
            CreateMap<PostViewModel, Post>();
            CreateMap<Post, PostViewModel>();
            CreateMap<Post, PostViewDto>();
        }
    }
}