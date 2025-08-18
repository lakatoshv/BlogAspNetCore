namespace Blog.FSharp.Core.Mapping

open System

/// <summary>
/// Types map.
/// Represents a mapping between source and destination types for AutoMapper.
/// </summary>
type TypesMap() =
    /// <summary>
    /// Gets or sets the source type.
    /// </summary>
    member val Source : Type = null with get, set

    /// <summary>
    /// Gets or sets the destination type.
    /// </summary>
    member val Destination : Type = null with get, set
