namespace Blog.FSharp.Contracts.V1.Responses

open System.Collections.Generic
open Blog.FSharp.Contracts

/// <summary>
/// Error response.
/// </summary>
type ErrorResponse() =
    /// <summary>
    /// Gets or sets the errors.
    /// </summary>
    member val Errors: List<ErrorModel> = List<ErrorModel>() with get, set

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResponse"/> class with one error.
    /// </summary>
    /// <param name="error">The error.</param>
    new (error: ErrorModel) as this =
        ErrorResponse() then
        this.Errors.Add(error)