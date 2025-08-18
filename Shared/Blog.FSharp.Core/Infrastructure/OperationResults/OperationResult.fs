namespace Blog.FSharp.Core.Infrastructure.OperationResults

open System
open System.Text
open System.Collections.Generic
open Blog.FSharp.Core.Infrastructure.OperationResults.Interfaces

/// <summary>
/// Cyclic types in F#: OperationResult references Metadata and vice versa.
/// </summary>
[<AbstractClass; Serializable>]
type OperationResult() =
    /// <summary>Logs.</summary>
    let logs = List<string>()

    /// <summary>Activity Id.</summary>
    member val ActivityId = OperationResult.generate 11 with get, set

    /// <summary>Metadata reference (mutable, option type to avoid cyclic reference issues).</summary>
    member val Metadata: Metadata option = None with get, set

    /// <summary>Exception (optional).</summary>
    member val Exception: exn option = None with get, set

    /// <summary>Logs enumerable.</summary>
    member this.Logs = logs :> seq<string>

    /// <summary>Append a single log entry.</summary>
    member this.AppendLog(message: string) =
        if not (String.IsNullOrEmpty message) then
            if message.Length > 500 then logs.Add(message.Substring(0,500))
            logs.Add(message)

    /// <summary>Append multiple log entries.</summary>
    member this.AppendLog(messages: seq<string>) =
        if not (isNull messages) then
            for m in messages do this.AppendLog(m)

    /// <summary>Generate random string of given length.</summary>
    static member private generate(size: int) =
        let chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray()
        let data = Array.zeroCreate<byte> size
        use rng = System.Security.Cryptography.RandomNumberGenerator.Create()
        rng.GetBytes(data)
        let sb = StringBuilder(size)
        for b in data do sb.Append(chars.[int b % chars.Length]) |> ignore
        sb.ToString()

and [<Serializable>] Metadata(source: OperationResult, message: string, ?metadataType: MetadataType) =
    /// <summary>Data object.</summary>
    let mutable dataObj: obj = null

    /// <summary>Metadata type.</summary>
    member val Type = defaultArg metadataType MetadataType.Info with get, set

    /// <summary>Message text.</summary>
    member val Message = message with get, set

    /// <summary>Reference to source OperationResult.</summary>
    member val Source = source with get

    /// <summary>Data object property.</summary>
    member this.DataObject
        with get() = dataObj
        and private set(v) = dataObj <- v

    interface IHaveDataObject with
        /// <summary>Add data object to metadata.</summary>
        member this.AddData(d: obj) =
            match d with
            | :? exn as ex when this.Source.Metadata.IsNone ->
                this.Source.Metadata <- Some (Metadata(this.Source, ex.Message))
            | _ ->
                match this.Source.Metadata with
                | Some m -> m.DataObject <- d
                | None -> dataObj <- d

/// <summary>Generic operation result with result value.</summary>
[<Serializable>]
type OperationResult<'T>() =
    inherit OperationResult()
    /// <summary>Result value.</summary>
    member val Result: 'T = Unchecked.defaultof<'T> with get, set

    /// <summary>True if result is OK (no errors and result is not default).</summary>
    member this.Ok =
        match this.Metadata with
        | None -> this.Exception.IsNone && not (Unchecked.equals this.Result Unchecked.defaultof<'T>)
        | Some m -> 
            this.Exception.IsNone &&
            not (Unchecked.equals this.Result Unchecked.defaultof<'T>) &&
            m.Type <> MetadataType.Error

/// <summary>OperationResult extension functions.</summary>
module OperationResultExtensions =
    /// <summary>Add info metadata.</summary>
    let addInfo (source: OperationResult) (message: string) =
        source.AppendLog(message)
        let metadata = Metadata(source, message, MetadataType.Info)
        source.Metadata <- Some metadata
        metadata :> IHaveDataObject

    /// <summary>Add success metadata.</summary>
    let addSuccess (source: OperationResult) (message: string) =
        source.AppendLog(message)
        let metadata = Metadata(source, message, MetadataType.Success)
        source.Metadata <- Some metadata
        metadata :> IHaveDataObject

    /// <summary>Add warning metadata.</summary>
    let addWarning (source: OperationResult) (message: string) =
        source.AppendLog(message)
        let metadata = Metadata(source, message, MetadataType.Warning)
        source.Metadata <- Some metadata
        metadata :> IHaveDataObject

    /// <summary>Add error metadata from message.</summary>
    let addError (source: OperationResult) (message: string) =
        source.AppendLog(message)
        let metadata = Metadata(source, message, MetadataType.Error)
        source.Metadata <- Some metadata
        metadata :> IHaveDataObject

    /// <summary>Add error metadata from exception.</summary>
    let addErrorEx (source: OperationResult) (ex: exn) =
        source.Exception <- Some ex
        let metadata = Metadata(source, ex.Message, MetadataType.Error)
        source.Metadata <- Some metadata
        if not (isNull ex) then source.AppendLog(ex.Message)
        metadata :> IHaveDataObject

    /// <summary>Add error metadata from message and exception.</summary>
    let addErrorMsgEx (source: OperationResult) (message: string) (ex: exn) =
        source.Exception <- Some ex
        let metadata = Metadata(source, message, MetadataType.Error)
        source.Metadata <- Some metadata
        if not (String.IsNullOrEmpty message) then source.AppendLog(message)
        if not (isNull ex) then source.AppendLog(ex.Message)
        metadata :> IHaveDataObject

    /// <summary>Get metadata messages and logs as string.</summary>
    let getMetadataMessages (source: OperationResult option) =
        match source with
        | None -> raise (ArgumentNullException("source"))
        | Some s ->
            let sb = StringBuilder()
            match s.Metadata with
            | Some m -> sb.AppendLine(m.Message |> Option.ofObj |> Option.defaultValue "") |> ignore
            | None -> ()
            s.Logs |> Seq.iter (fun log -> sb.AppendLine("Log: " + log) |> ignore)
            sb.ToString()
