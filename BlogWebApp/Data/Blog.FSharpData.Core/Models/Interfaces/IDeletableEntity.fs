namespace Blog.FSharpData.Core.Models.Interfaces

open System

/// Deletable entity interface
type IDeletableEntity =
    abstract member IsDeleted: bool with get, set
    abstract member DeletedOn: Nullable<DateTime> with get, set
