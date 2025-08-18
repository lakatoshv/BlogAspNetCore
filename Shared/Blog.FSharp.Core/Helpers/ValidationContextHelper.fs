namespace Blog.FSharp.Core.Helpers

open System.Collections.ObjectModel
open System.ComponentModel.DataAnnotations

/// <summary>
/// ValidationContext Helper for validation operations.
/// </summary>
module ValidationContextHelper =

    /// <summary>
    /// Tries the validate.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="results">The results.</param>
    /// <param name="validationContext">Optional validation context.</param>
    /// <returns>bool indicating success or failure.</returns>
    let tryValidate (obj: obj) (validationContext: ValidationContext option) =
        let context =
            match validationContext with
            | Some ctx -> ctx
            | None -> ValidationContext(obj, serviceProvider = null, items = null)
        
        let results = Collection<ValidationResult>()
        let success = Validator.TryValidateObject(obj, context, results, validateAllProperties = true)
        success, results
