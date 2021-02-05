using Blog.Contracts.V1.Responses.UsersResponses;
using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Profile = AutoMapper.Profile;

namespace Blog.Web.Mappers.AspNetUser
{
    public class VIewModelToEntityMappingRole : Profile
    {
        public VIewModelToEntityMappingRole()
        {
            CreateMap<IdentityUserRole<string>, RoleResponse>();
            CreateMap<ApplicationRole, RoleResponse>();
        }
    }
}