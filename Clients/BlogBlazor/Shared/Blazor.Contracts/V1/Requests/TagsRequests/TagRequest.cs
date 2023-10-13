namespace Blazor.Contracts.V1.Requests.TagsRequests;

using Interfaces;

/// <summary>
/// Tag request.
/// </summary>
public class TagRequest : IRequest
{
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string Title { get; set; }
}