namespace Blog.Web.SwaggerExamples.Requests.TagsRequests;

using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Requests.TagsRequests;
using Core.Consts;

/// <summary>
/// Create tag request example.
/// </summary>
/// <seealso cref="IExamplesProvider{CreateTagRequest}" />
public class CreateTagRequestExample : IExamplesProvider<CreateTagRequest>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public CreateTagRequest GetExamples()
    {
        return new CreateTagRequest
        {
            Title = SwaggerExamplesConsts.CreateTagRequestExample.Title,
        };
    }
}