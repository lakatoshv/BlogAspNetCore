namespace Blog.FSharp.Core.Infrastructure.OperationResults.Interfaces

/// <summary>
/// Have data object interface.
/// </summary>
type IHaveDataObject =
    /// <summary>
    /// Adds the data.
    /// </summary>
    /// <param name="data">The data.</param>
    abstract member AddData : data: obj -> unit