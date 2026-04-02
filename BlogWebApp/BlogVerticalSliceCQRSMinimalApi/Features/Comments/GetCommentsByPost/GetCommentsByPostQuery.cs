using Blog.Contracts.V1.Responses.CommentsResponses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetCommentsByPost;

public record GetCommentsByPostQuery(int PostId, string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayType)
    : SortParametersQuery(OrderBy, SortBy, CurrentPage, PageSize, DisplayType), IQuery<PagedCommentsResponse>;