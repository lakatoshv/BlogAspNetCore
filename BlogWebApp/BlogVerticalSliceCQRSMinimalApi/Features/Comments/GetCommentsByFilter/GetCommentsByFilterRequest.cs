namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetCommentsByFilter;

public record GetCommentsByFilterRequest(string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayTypel);
