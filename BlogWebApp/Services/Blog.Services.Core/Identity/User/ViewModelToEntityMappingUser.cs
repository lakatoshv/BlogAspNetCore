// <copyright file="ViewModelToEntityMappingUser.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Identity.User;

using Blog.Services.Core.Dtos.User;
using Contracts.V1.Requests.UsersRequests;
using Contracts.V1.Responses.UsersResponses;
using Data.Models;
using System.Linq;

/// <summary>
/// View model to entity mapping user.
/// </summary>
public class ViewModelToEntityMappingUser : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelToEntityMappingUser"/> class.
    /// </summary>
    public ViewModelToEntityMappingUser()
    {
        CreateMap<UserRegistrationDto, ApplicationUser>();
        CreateMap<ApplicationUserDto, ApplicationUser>();
        CreateMap<ApplicationUserDto, ApplicationUserResponse>();
        CreateMap<RegistrationRequest, ApplicationUser>()
            .ForMember(destinationMember => destinationMember.Roles, memberOptions
                => memberOptions.Ignore());
        CreateMap<ApplicationUser, ApplicationUserDto>();
        CreateMap<ApplicationUser, AccountResponse>()
            .ForMember(destinationMember => destinationMember.Roles, memberOptions
                => memberOptions.MapFrom(src => src.Roles.Select(x => new RoleResponse { Id = x.RoleId })));
    }
}