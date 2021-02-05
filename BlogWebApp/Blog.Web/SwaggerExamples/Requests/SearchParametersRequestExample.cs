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
                Search = SwaggerExamplesConsts.SearchParametersRequestExample.Search,
                SortParameters = new SortParametersRequest
                {
                    OrderBy = SwaggerExamplesConsts.SortParametersRequestExample.OrderBy,
                    SortBy = SwaggerExamplesConsts.SortParametersRequestExample.SortBy,
                    CurrentPage = SwaggerExamplesConsts.SortParametersRequestExample.CurrentPage,
                    PageSize = SwaggerExamplesConsts.SortParametersRequestExample.PageSize,
                },
            };
        }
    }
}
