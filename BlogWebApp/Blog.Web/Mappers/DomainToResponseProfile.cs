using Blog.Data.Models;
using Blog.Web.Contracts.V1.Responses.UsersResponses;

namespace Blog.Web.Mappers
{
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
        }
    }
}