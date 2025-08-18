namespace Blog.FSharp.Core.TableFilters

/// <summary>
/// Table search model.
/// </summary>
type TableSearchModel() =
    /// <summary>
    /// Gets or sets value.
    /// </summary>
    member val Value : string = null with get, set

    /// <summary>
    /// Gets or sets a value indicating whether regex.
    /// </summary>
    member val Regex : bool = false with get, set