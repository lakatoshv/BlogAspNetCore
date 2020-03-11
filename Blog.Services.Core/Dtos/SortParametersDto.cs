namespace Blog.Services.Core.Dtos
{
    public class SortParametersDto
    {
        public string OrderBy { get; set; }
        public string SortBy { get; set; }
        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
        public string DisplayType { get; set; }
    }
}
