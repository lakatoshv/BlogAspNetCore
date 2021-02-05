using Blog.Contracts.V1.Requests;
using Blog.Core.Consts;
using Swashbuckle.AspNetCore.Filters;

namespace Blog.Web.SwaggerExamples.Requests
{
    /// <summary>
    /// Search parameters request example.
    /// </summary>
    /// <seealso cref="IExamplesProvider{SearchParametersRequest}" />
    public class SearchParametersRequestExample : IExamplesProvider<SearchParametersRequest>
    {
        /// <inheritdoc cref="IExamplesProvider{T}"/>
        public SearchParametersRequest GetExamples()
        {
            return new SearchParametersRequest
            {
                Search = SwaggerExamplesConsts.SearchParametersRequest.Search,
                SortParameters = new SortParametersRequest
                {
                    OrderBy = SwaggerExamplesConsts.SortParametersRequest.OrderBy,
                    SortBy = SwaggerExamplesConsts.SortParametersRequest.SortBy,
                    CurrentPage = SwaggerExamplesConsts.SortParametersRequest.CurrentPage,
                    PageSize = SwaggerExamplesConsts.SortParametersRequest.PageSize,
                },
            };
        }
    }
}
