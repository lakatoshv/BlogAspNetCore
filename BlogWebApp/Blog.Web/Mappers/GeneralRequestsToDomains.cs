using AutoMapper;
using Blog.Services.Core.Dtos;
using Blog.Contracts.V1.Requests;
using Blog.Contracts.V1.Responses;
using Blog.Core.Helpers;

namespace Blog.Web.Mappers
{
    /// <summary>
    /// General requests to domains.
    /// </summary>
    /// <seealso cref="Profile" />
    public class GeneralRequestsToDomains : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralRequestsToDomains"/> class.
        /// </summary>
        public GeneralRequestsToDomains()
        {
            CreateMap<SearchParametersRequest, SearchParametersDto>();
            CreateMap<SortParametersRequest, SortParametersDto>();
            CreateMap<PageInfo, PageInfoResponse>();
        }
    }
}