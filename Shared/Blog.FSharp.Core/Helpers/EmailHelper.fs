namespace Blog.FSharp.Core.Helpers

open System
open System.Text.RegularExpressions

/// <summary>
/// Email validation helper.
/// </summary>
module EmailHelper =

    /// <summary>
    /// Checks if a string is a valid email.
    /// </summary>
    let private isValidEmail (email: string) : bool =
        if String.IsNullOrWhiteSpace(email) then false
        else
            let pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
            Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase)

    /// <summary>
    /// Validates email entries.
    /// </summary>
    /// <param name="emails">A string with emails separated by ; | , or space.</param>
    /// <returns>Sequence of valid emails.</returns>
    let getValidEmails (emails: string) : seq<string> =
        if String.IsNullOrWhiteSpace(emails) then
            Seq.empty
        else
            emails.Split([| ';'; '|'; ' '; ',' |], StringSplitOptions.RemoveEmptyEntries)
            |> Seq.map (fun x -> x.Trim())      // remove extra spaces
            |> Seq.filter isValidEmail          // use our regex validation
