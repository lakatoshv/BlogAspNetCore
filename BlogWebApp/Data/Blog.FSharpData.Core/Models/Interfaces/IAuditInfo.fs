namespace Blog.FSharpData.Core.Models.Interfaces

open System

/// Audit info interface
type IAuditInfo =
    abstract member CreatedOn: DateTime with get, set
    abstract member ModifiedOn: Nullable<DateTime> with get, set

