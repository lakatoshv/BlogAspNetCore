namespace Blog.FSharp.Core.Attributes.SwaggerAttributes

open System

/// <summary>
/// Swagger controller group attribute.
/// </summary>
[<AttributeUsage(AttributeTargets.Class)>]
type SwaggerGroupAttribute(groupName: string) =
    inherit Attribute()

    /// <summary>
    /// Gets the name of the group.
    /// </summary>
    member val GroupName: string = groupName with get
