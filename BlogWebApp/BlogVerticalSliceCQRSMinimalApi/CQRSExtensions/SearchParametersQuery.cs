namespace BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

public record SearchParametersQuery(string OrderBy, string SortBy, int? CurrentPage, int? PageSize, string DisplayType)
    : SortParametersQuery(OrderBy, SortBy, CurrentPage, PageSize, DisplayType);