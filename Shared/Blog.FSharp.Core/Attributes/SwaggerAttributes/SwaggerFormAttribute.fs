namespace Blog.FSharp.Core.Attributes.SwaggerAttributes

open System

/// <summary>
/// Custom attribute for Swagger Upload Form.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SwaggerFormAttribute"/> class.
/// Custom attribute for Swagger Upload Form.
/// </remarks>
/// <seealso cref="Attribute" />
/// <inheritdoc cref="Attribute"/>
[<AttributeUsage(AttributeTargets.Method)>]
type SwaggerFormAttribute(parameterName: string, description: string, ?hasFileUpload: bool) =
    inherit Attribute()

    /// Gets a value indicating whether this instance has file upload.
    member val HasFileUpload: bool = defaultArg hasFileUpload true with get

    /// Gets the name of the parameter IFormFile.
    member val ParameterName: string = parameterName with get

    /// Gets the description.
    member val Description: string = description with get

