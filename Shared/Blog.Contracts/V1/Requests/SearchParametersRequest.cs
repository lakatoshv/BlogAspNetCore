namespace Blog.Contracts.V1.Requests;

using Interfaces;

/// <summary>
/// Search parameters request.
/// </summary>
public class SearchParametersRequest : IRequest
{
    /// <summary>
    /// Gets or sets search.
    /// </summary>
    public string Search { get; set; }

    /// <summary>
    /// Gets or sets the sort parameters.
    /// </summary>
    /// <value>
    /// The sort parameters.
    /// </value>
    public SortParametersRequest SortParameters { get; set; }
}