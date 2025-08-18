namespace Blog.FSharp.Core.Helpers

open System
open System.Collections.Generic
open System.ComponentModel.DataAnnotations
open System.Reflection

/// <summary>
/// Enum Helper.
/// </summary>
module EnumHelper =

    /// <summary>
    /// Get all enum values as a list.
    /// </summary>
    let getValues<'T when 'T : struct and 'T : enum<'T> and 'T : equality> () : 'T list =
        Enum.GetValues(typeof<'T>) :?> 'T[]
        |> Array.toList

    /// <summary>
    /// Get all enum names as list of strings.
    /// </summary>
    let getNames<'T when 'T : struct and 'T : enum<'T> and 'T : equality> () : string list =
        typeof<'T>.GetFields(BindingFlags.Static ||| BindingFlags.Public)
        |> Array.map (fun fi -> fi.Name)
        |> Array.toList

    /// <summary>
    /// Parse string to enum.
    /// </summary>
    let parse<'T when 'T : struct and 'T : enum<'T> and 'T : equality> (value: string) : 'T =
        Enum.Parse(typeof<'T>, value, true) :?> 'T

    /// <summary>
    /// Try parse string to enum.
    /// </summary>
    let tryParse<'T when 'T : struct and 'T : enum<'T> 
                                     and 'T : (new: unit -> 'T) 
                                     and 'T :> ValueType> (value: string) : 'T option =
        let mutable result = Unchecked.defaultof<'T>
        let success = Enum.TryParse<'T>(value, true, &result)
        if success then Some result else None

    /// <summary>
    /// Get display name for an enum value, using DisplayAttribute if exists.
    /// </summary>
    let getDisplayValue<'T when 'T : struct and 'T : enum<'T> and 'T : equality> (value: 'T) : string =
        let fi = value.GetType().GetField(value.ToString())
        if isNull fi then ""
        else
            let attr = fi.GetCustomAttributes(typeof<DisplayAttribute>, false) |> Seq.tryHead
            match attr with
            | Some(:? DisplayAttribute as da) when not (isNull da.Name) -> da.Name
            | _ -> value.ToString()

    /// <summary>
    /// Get dictionary of enum values with display names.
    /// </summary>
    let getValuesWithDisplayNames<'T when 'T : struct and 'T : enum<'T> and 'T : equality> () : Dictionary<'T,string> =
        let dict = Dictionary<'T,string>()
        getValues<'T> ()
        |> List.iter (fun v -> dict.[v] <- getDisplayValue v)
        dict

    /// <summary>
    /// Get list of display names for enum type.
    /// </summary>
    let getDisplayValues<'T when 'T : struct and 'T : enum<'T> and 'T : equality> () : string list =
        getValues<'T> ()
        |> List.map getDisplayValue
