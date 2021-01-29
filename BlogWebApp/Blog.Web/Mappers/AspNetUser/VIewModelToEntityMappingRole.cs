using AutoMapper;
using Blog.Contracts.V1.Responses.UsersResponses;
using Microsoft.AspNetCore.Identity;

namespace Blog.Web.Mappers.AspNetUser
{
    public class VIewModelToEntityMappingRole : Profile
    {
        public VIewModelToEntityMappingRole()
        {
            CreateMap<IdentityUserRole<string>, RoleResponse>();
        }
    }
}