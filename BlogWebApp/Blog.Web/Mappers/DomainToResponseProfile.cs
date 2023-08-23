namespace Blog.Web.Mappers;

using Data.Models;
using Contracts.V1.Responses.UsersResponses;

/// <summary>
/// Domain to response profile.
/// </summary>
/// <seealso cref="AutoMapper.Profile" />
public class DomainToResponseProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainToResponseProfile"/> class.
    /// </summary>
    public DomainToResponseProfile()
    {
        CreateMap<Profile, ProfileResponse>();
        CreateMap<ApplicationUser, ApplicationUserResponse>();
    }
}