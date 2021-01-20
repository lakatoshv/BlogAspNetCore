using System.Linq;
using Blog.Web.Contracts.V1.Requests.UsersRequests;
using Blog.Web.Contracts.V1.Responses.UsersResponses;

namespace Blog.Web.Mappers.AspNetUser
{
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
            CreateMap<ApplicationUser, UserItemViewModel>();
            CreateMap<UserItemViewModel, ApplicationUser>();
            CreateMap<UserRegistrationDto, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<ApplicationUserDto, ApplicationUser>();

            CreateMap<ApplicationUser, AccountResponse>()
                .ForMember(destinationMember => destinationMember.Roles, memberOptions
                => memberOptions.MapFrom(src => src.Roles.Select(x => new RoleResponse { Id = x.RoleId })));

            CreateMap<RegistrationRequest, ApplicationUser>()
                .ForMember(destinationMember => destinationMember.Roles, memberOptions
                    => memberOptions.Ignore());
        }
    }
}
