namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.GetPosts;

public class UserPostsRequest(string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayType, string Tag);