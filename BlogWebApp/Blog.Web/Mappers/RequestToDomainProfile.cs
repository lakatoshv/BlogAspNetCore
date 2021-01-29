using Blog.Data.Models;
using Blog.Web.Contracts.V1.Responses.UsersResponses;

namespace Blog.Web.Mappers
{
    public class RequestToDomainProfile : AutoMapper.Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<Profile, ProfileResponse>();
        }
    }
}