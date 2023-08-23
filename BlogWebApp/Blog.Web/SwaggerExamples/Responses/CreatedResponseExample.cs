namespace Blog.Web.SwaggerExamples.Responses;

using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses;

/// <summary>
/// Created response example.
/// </summary>
/// <seealso cref="IExamplesProvider{CreatedResponse{int}}" />
public class CreatedResponseExample : IExamplesProvider<CreatedResponse<int>>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public CreatedResponse<int> GetExamples()
    {
        return new CreatedResponse<int>
        {
            Id = 0,
        };
    }
}