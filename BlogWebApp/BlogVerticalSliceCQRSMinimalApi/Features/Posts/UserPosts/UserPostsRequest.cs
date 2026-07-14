namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.UserPosts;

public class UserPostsRequest(string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayType, string Tag, string Id);