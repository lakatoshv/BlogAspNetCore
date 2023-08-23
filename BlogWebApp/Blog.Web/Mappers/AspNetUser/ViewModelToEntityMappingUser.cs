namespace Blog.Web.Mappers.AspNetUser;

using System.Linq;
using Contracts.V1.Requests.UsersRequests;
using Contracts.V1.Responses.UsersResponses;
using Data.Models;
using Blog.Services.Core.Dtos.User;
using Blog.Web.ViewModels.AspNetUser;

/// <summary>
/// View model to entity mapping user.
/// </summary>
public class ViewModelToEntityMappingUser : AutoMapper.Profile
{
    /// <summary>
    /// Initializes static members of the <see cref="ViewModelToEntityMappingUser"/> class.
    /// </summary>
    public ViewModelToEntityMappingUser()
    {
        CreateMap<UserItemViewModel, ApplicationUser>();
        CreateMap<UserRegistrationDto, ApplicationUser>();
        CreateMap<ApplicationUserDto, ApplicationUser>();
        CreateMap<ApplicationUserDto, ApplicationUserResponse>();
        CreateMap<RegistrationRequest, ApplicationUser>()
            .ForMember(destinationMember => destinationMember.Roles, memberOptions
                => memberOptions.Ignore());
        CreateMap<ApplicationUser, UserItemViewModel>();
        CreateMap<ApplicationUser, ApplicationUserDto>();
        CreateMap<ApplicationUser, AccountResponse>()
            .ForMember(destinationMember => destinationMember.Roles, memberOptions
                => memberOptions.MapFrom(src => src.Roles.Select(x => new RoleResponse { Id = x.RoleId })));
    }
}