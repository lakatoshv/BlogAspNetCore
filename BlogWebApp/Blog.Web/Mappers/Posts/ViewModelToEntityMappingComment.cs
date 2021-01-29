﻿using Blog.Data.Models;
using Blog.Services.Core.Dtos.Posts;
using Blog.Contracts.V1.Requests.CommentsRequests;
using Blog.Contracts.V1.Responses.CommentsResponses;

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
            CreateMap<CommentsViewDto, PagedCommentsResponse>();
        }
    }
}