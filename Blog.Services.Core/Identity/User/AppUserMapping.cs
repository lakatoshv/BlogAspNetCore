namespace Blog.Services.Core.Identity.User
{
    using Blog.Data.Models;

    public class ViewModelToEntityMappingUser : AutoMapper.Profile
    {
        public ViewModelToEntityMappingUser()
        {
            CreateMap<ApplicationUser, ApplicationUser>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(s => s.UserName))
                .ForMember(x => x.Email, opt => opt.MapFrom(s => s.Email))
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(s => s.PhoneNumber))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
