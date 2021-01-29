using Blog.Data.Models;
using Blog.Services.Core.Dtos.Posts;
using Blog.Contracts.V1.Requests.PostsRequests;
using Blog.Contracts.V1.Responses.PostsResponses;

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
            CreateMap<Post, PostViewDto>();
            CreateMap<Post, PostResponse>();
            CreateMap<PostViewDto, PostViewResponse>();
            CreateMap<PostsViewDto, PagedPostsResponse>();
            CreateMap<PostShowViewDto, PostWithPagedCommentsResponse>();
            CreateMap<CreatePostRequest, Post>();
            CreateMap<UpdatePostRequest, Post>();
        }
    }
}