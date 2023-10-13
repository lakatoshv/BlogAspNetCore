namespace Blazor.Contracts.V1.Requests.TagsRequests;

using Interfaces;

/// <summary>
/// Update tag request.
/// </summary>
public class UpdateTagRequest : IRequest
{
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string Title { get; set; }
}