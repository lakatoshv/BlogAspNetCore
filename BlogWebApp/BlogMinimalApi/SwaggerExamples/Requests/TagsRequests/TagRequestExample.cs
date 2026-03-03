namespace BlogMinimalApi.SwaggerExamples.Requests.TagsRequests;

using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Requests.TagsRequests;
using Blog.Core.Consts;

/// <summary>
/// Tag request example.
/// </summary>
/// <seealso cref="IExamplesProvider{TagRequest}" />
public class TagRequestExample : IExamplesProvider<TagRequest>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public TagRequest GetExamples()
    {
        return new TagRequest
        {
            Title = SwaggerExamplesConsts.TagRequestExample.Title
        };
    }
}