namespace Blog.Web.Mappers;

using Data.Models;
using Contracts.V1.Responses.UsersResponses;

/// <summary>
/// Request to domain profile.
/// </summary>
/// <seealso cref="AutoMapper.Profile" />
public class RequestToDomainProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestToDomainProfile"/> class.
    /// </summary>
    public RequestToDomainProfile()
    {
        CreateMap<Profile, ProfileResponse>();
    }
}