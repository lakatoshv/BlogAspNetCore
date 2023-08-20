namespace Blog.Core.Attributes.SwaggerAttributes;

using System;

/// <summary>
/// Swagger controller group attribute.
/// </summary>
/// <seealso cref="System.Attribute" />
[AttributeUsage(AttributeTargets.Class)]
public class SwaggerGroupAttribute : Attribute
{
    /// <inheritdoc cref="Attribute"/>
    public SwaggerGroupAttribute(string groupName)
    {
        GroupName = groupName;
    }

    /// <summary>
    /// Gets the name of the group.
    /// </summary>
    /// <value>
    /// The name of the group.
    /// </value>
    public string GroupName { get; }
}