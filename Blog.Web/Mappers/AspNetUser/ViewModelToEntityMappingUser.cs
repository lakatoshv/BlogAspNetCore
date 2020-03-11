namespace Blog.Web.Mappers.AspNetUser
{
    using Blog.Data.Models;
    using Blog.Services.Core.Dtos.User;
    using Blog.Web.ViewModels.AspNetUser;

    public class ViewModelToEntityMappingUser : AutoMapper.Profile
    {
        public ViewModelToEntityMappingUser()
        {
            CreateMap<ApplicationUser, UserItemViewModel>();
            CreateMap<UserItemViewModel, ApplicationUser>();
            CreateMap<UserRegistrationDto, ApplicationUser>();
        }
    }
}
