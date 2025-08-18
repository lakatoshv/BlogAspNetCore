namespace Blog.FSharp.Core.Helpers

open System
open System.Text

/// <summary>
/// Exception helper.
/// </summary>
module ExceptionHelper =

    /// <summary>
    /// Recursively gets the error message from the exception and its inner exceptions.
    /// </summary>
    /// <param name="ex">The exception.</param>
    /// <returns>All messages as a single string.</returns>
    let rec private getErrorMessage (ex: Exception) : string =
        let sb = StringBuilder()
        sb.AppendLine(ex.Message) |> ignore
        if not (isNull ex.InnerException) then
            // Явно вказуємо, що результат getErrorMessage – string
            let innerMsg: string = getErrorMessage ex.InnerException
            sb.AppendLine(innerMsg) |> ignore
        sb.ToString()

    /// <summary>
    /// Gets the messages.
    /// </summary>
    /// <param name="ex">The exception.</param>
    /// <returns>string.</returns>
    let getMessages (ex: Exception) : string =
        if isNull ex then "Exception is NULL"
        else getErrorMessage ex