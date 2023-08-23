namespace Blog.Web.SwaggerExamples.Requests;

using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Requests;
using Core.Consts;

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
            OrderBy = SwaggerExamplesConsts.SortParametersRequestExample.OrderBy,
            SortBy = SwaggerExamplesConsts.SortParametersRequestExample.SortBy,
            CurrentPage = SwaggerExamplesConsts.SortParametersRequestExample.CurrentPage,
            PageSize = SwaggerExamplesConsts.SortParametersRequestExample.PageSize,
        };
    }
}