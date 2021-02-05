using Blog.Contracts.V1.Requests.TagsRequests;
using Blog.Core.Consts;
using Swashbuckle.AspNetCore.Filters;

namespace Blog.Web.SwaggerExamples.Requests.TagsRequests
{
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
}
