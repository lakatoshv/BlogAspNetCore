namespace Blazor.Contracts.V1.Requests.TagsRequests;

using Interfaces;

/// <summary>
/// Create tag request.
/// </summary>
public class CreateTagRequest : IRequest
{
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string Title { get; set; }
}