using AutoMapper;
using Blog.Services.Core.Dtos;
using Blog.Web.Contracts.V1.Requests;

namespace Blog.Web.Mappers
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<SortParametersRequest, SortParametersDto>();
        }
    }
}