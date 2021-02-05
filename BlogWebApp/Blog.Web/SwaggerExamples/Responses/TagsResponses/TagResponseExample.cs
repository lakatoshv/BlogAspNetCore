using Blog.Contracts.V1.Responses.TagsResponses;
using Blog.Core.Consts;
using Swashbuckle.AspNetCore.Filters;

namespace Blog.Web.SwaggerExamples.Responses.TagsResponses
{
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
}
