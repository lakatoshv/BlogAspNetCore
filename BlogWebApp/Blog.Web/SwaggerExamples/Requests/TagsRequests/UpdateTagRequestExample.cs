namespace Blog.Web.SwaggerExamples.Requests.TagsRequests;

using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Requests.TagsRequests;
using Core.Consts;

/// <summary>
/// Update tag request example.
/// </summary>
/// <seealso cref="IExamplesProvider{CreateCommentRequest}" />
public class UpdateTagRequestExample : IExamplesProvider<UpdateTagRequest>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public UpdateTagRequest GetExamples()
    {
        return new UpdateTagRequest
        {
            Title = SwaggerExamplesConsts.UpdateTagRequestExample.Title
        };
    }
}