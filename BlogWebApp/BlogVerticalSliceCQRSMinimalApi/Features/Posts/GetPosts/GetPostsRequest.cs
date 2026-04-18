namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.GetPosts;

public class GetPostsRequest(string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayType, string Tag);