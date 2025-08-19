namespace Blog.FSharp.Contracts.V1.Requests.CategoriesRequests

open System.ComponentModel.DataAnnotations
open Blog.FSharp.Contracts.V1.Requests.Interfaces

/// <summary>
/// Update category request.
/// </summary>
type UpdateCategoryRequest() =
    interface IRequest

    /// <summary>
    /// Gets or sets the parent category id.
    /// </summary>
    member val ParentCategoryId: int option = None with get, set

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [<Required>]
    member val Name: string = "" with get, set

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    member val Description: string = "" with get, set