﻿namespace Blog.Web.Mappers.Posts;

using Data.Models;
using Blog.Services.Core.Dtos.Posts;
using Contracts.V1.Requests.TagsRequests;
using Contracts.V1.Responses.TagsResponses;

/// <summary>
/// View model to entity mapping tag.
/// </summary>
/// <seealso cref="AutoMapper.Profile" />
public class ViewModelToEntityMappingTag : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelToEntityMappingTag"/> class.
    /// </summary>
    public ViewModelToEntityMappingTag()
    {
        CreateMap<TagViewDto, TagResponse>();
        CreateMap<Tag, TagResponse>();
        CreateMap<TagsViewDto, PagedTagsResponse>();
        CreateMap<TagRequest, Tag>();
        CreateMap<CreateTagRequest, Tag>();
        CreateMap<UpdateTagRequest, Tag>();
    }
}