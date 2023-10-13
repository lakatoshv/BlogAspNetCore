namespace Blog.Web.SwaggerExamples.Responses;

using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses;

/// <summary>
/// Page info response example.
/// </summary>
/// <seealso cref="IExamplesProvider{PageInfoResponse}" />
public class PageInfoResponseExample : IExamplesProvider<PageInfoResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public PageInfoResponse GetExamples()
    {
        return new PageInfoResponse
        {
            PageNumber = 1,
            PageSize = 10,
            TotalItems = 100
        };
    }
}