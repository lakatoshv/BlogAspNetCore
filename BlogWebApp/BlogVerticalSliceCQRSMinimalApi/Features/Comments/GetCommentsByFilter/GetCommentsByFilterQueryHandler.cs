using AutoMapper;
using Blog.Contracts.V1.Responses.CommentsResponses;
using Blog.EntityServices.Interfaces;
using Blog.Services.Core.Dtos;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetCommentsByFilter;

public class GetCommentsByFilterQueryHandler(
    ICommentsService service,
    IMapper mapper)
    : IQueryHandler<GetCommentsByFilterQuery, PagedCommentsResponse>
{
    public async Task<PagedCommentsResponse> Handle(GetCommentsByFilterQuery query, CancellationToken cancellationToken)
    {
        query = query with { CurrentPage = query.CurrentPage ?? 1 };
        query = query with { PageSize = 10 };

        var result = await service.GetPagedComments(
            mapper.Map<SortParametersDto>(query));

        return mapper.Map<PagedCommentsResponse>(result);
    }
}