﻿namespace Blog.Web.Mappers.AspNetUser
{
    using Blog.Data.Models;
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
        }
    }
}
