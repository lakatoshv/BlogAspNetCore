using Blog.Contracts.V1.Responses.CommentsResponses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetCommentsByFilter;

public record GetCommentsByFilterQuery(string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayType)
    : SortParametersQuery(OrderBy, SortBy, CurrentPage, PageSize, DisplayType), IQuery<PagedCommentsResponse>;