using Blog.Contracts.V1.Requests;
using Blog.Core.Consts;
using Swashbuckle.AspNetCore.Filters;

namespace Blog.Web.SwaggerExamples.Requests
{
    /// <summary>
    /// Sort parameters request example.
    /// </summary>
    /// <seealso cref="IExamplesProvider{SortParametersRequest}" />
    public class SortParametersRequestExample : IExamplesProvider<SortParametersRequest>
    {
        /// <inheritdoc cref="IExamplesProvider{T}"/>
        public SortParametersRequest GetExamples()
        {
            return new SortParametersRequest
            {
                OrderBy = SwaggerExamplesConsts.SortParametersRequest.OrderBy,
                SortBy = SwaggerExamplesConsts.SortParametersRequest.SortBy,
                CurrentPage = SwaggerExamplesConsts.SortParametersRequest.CurrentPage,
                PageSize = SwaggerExamplesConsts.SortParametersRequest.PageSize,
            };
        }
    }
}
