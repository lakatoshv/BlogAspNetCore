namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetPagedComments;

public record GetCommentsByFilterRequest(string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayTypel);
