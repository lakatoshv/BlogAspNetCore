namespace Blazor.Contracts.V1.Responses;

using System;

/// <summary>
/// Page info response.
/// </summary>
public class PageInfoResponse
{
    /// <summary>
    /// Gets or sets current page number.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets page size.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets total items count.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// Gets total pages count.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((decimal)this.TotalItems / this.PageSize);
}