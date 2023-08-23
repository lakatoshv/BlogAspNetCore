namespace Blog.Web.SwaggerExamples.Responses.TagsResponses;

using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses.TagsResponses;
using Core.Consts;

/// <summary>
/// Tag response example.
/// </summary>
/// <seealso cref="IExamplesProvider{TagResponse}" />
public class TagResponseExample : IExamplesProvider<TagResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public TagResponse GetExamples()
    {
        return new TagResponse
        {
            Id = 0,
            Title = SwaggerExamplesConsts.TagResponseExample.Title + "1",
        };
    }
}