namespace Blog.FSharp.Core.Mapping.Interfaces

open AutoMapper

/// <summary>
/// Custom mappings interface.
/// </summary>
type ICustomMappings =
    /// <summary>
    /// Create mappings.
    /// </summary>
    /// <param name="configuration">configuration.</param>
    abstract member CreateMappings: configuration: IMapperConfigurationExpression -> unit

