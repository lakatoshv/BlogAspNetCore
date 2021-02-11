// <copyright file="ViewModelToEntityMappingUser.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Identity.User
{
    using Blog.Data.Models;

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
            this.CreateMap<ApplicationUser, ApplicationUser>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(s => s.UserName))
                .ForMember(x => x.Email, opt => opt.MapFrom(s => s.Email))
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(s => s.PhoneNumber))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
