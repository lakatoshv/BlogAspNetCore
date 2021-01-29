using Blog.Data.Models;
using Blog.Contracts.V1.Responses.UsersResponses;

namespace Blog.Web.Mappers
{
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
}