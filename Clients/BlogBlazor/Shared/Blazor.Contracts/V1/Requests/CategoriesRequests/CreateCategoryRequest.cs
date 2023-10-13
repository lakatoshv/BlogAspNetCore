using Blazor.Contracts.V1.Requests.Interfaces;

namespace Blazor.Contracts.V1.Requests.CategoriesRequests;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Create category request.
/// </summary>
public class CreateCategoryRequest : IRequest
{
    /// <summary>
    /// Gets or sets the parent category id.
    /// </summary>
    public int? ParentCategoryId { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>
    /// The description.
    /// </value>
    public string Description { get; set; }
}