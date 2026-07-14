using Blog.Contracts.V1.Responses.CommentsResponses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetCommentsByFilter;

public record GetCommentsByFilterCommand(string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayType)
    : ICommand<PagedCommentsResponse>;
