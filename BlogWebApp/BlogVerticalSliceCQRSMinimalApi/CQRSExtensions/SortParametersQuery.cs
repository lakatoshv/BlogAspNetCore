namespace BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

public record SortParametersQuery(string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayType);