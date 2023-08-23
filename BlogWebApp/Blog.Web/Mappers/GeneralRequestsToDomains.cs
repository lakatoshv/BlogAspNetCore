namespace Blog.Web.Mappers;

using AutoMapper;
using Blog.Services.Core.Dtos;
using Contracts.V1.Requests;
using Contracts.V1.Responses;
using Core.Helpers;

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