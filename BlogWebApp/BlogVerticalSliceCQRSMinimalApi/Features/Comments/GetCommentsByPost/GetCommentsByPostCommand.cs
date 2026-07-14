namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetCommentsByPost;

public record GetCommentsByPostRequest(int PostId, string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayType);