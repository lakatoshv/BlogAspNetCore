namespace Blog.Web.Mappers.AspNetUser;

using Microsoft.AspNetCore.Identity;
using Profile = AutoMapper.Profile;
using Contracts.V1.Responses.UsersResponses;
using Data.Models;

/// <summary>
/// VIew model to entity mapping role.
/// </summary>
/// <seealso cref="Profile" />
public class VIewModelToEntityMappingRole : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VIewModelToEntityMappingRole"/> class.
    /// </summary>
    public VIewModelToEntityMappingRole()
    {
        CreateMap<IdentityUserRole<string>, RoleResponse>();
        CreateMap<ApplicationRole, RoleResponse>();
    }
}