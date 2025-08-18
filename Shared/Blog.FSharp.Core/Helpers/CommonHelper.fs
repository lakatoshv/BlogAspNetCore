namespace Blog.FSharp.Core.Helpers

open System
open System.Collections.Generic
open System.ComponentModel
open System.Globalization
open System.Linq
open System.Net
open System.Reflection
open System.Security.Cryptography
open System.Text.RegularExpressions
open Blog.FSharp.Core.Infrastructure

/// <summary>
/// Common Helper.
/// </summary>
[<AbstractClass; Sealed>]
type CommonHelper private () =

    // Email regex
    static let emailExpression = 
        @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$"

    static let emailRegex = Regex(emailExpression, RegexOptions.IgnoreCase)

    /// Business days
    static member BusinessDays(firstDay: DateTime, lastDay: DateTime, [<ParamArray>] bankHolidays: DateTime[]) =
        let firstDay = firstDay.Date
        let lastDay = lastDay.Date
        if firstDay > lastDay then
            raise (ArgumentException("Incorrect last day " + lastDay.ToString()))

        let span = lastDay - firstDay
        let mutable businessDays = span.Days + 1
        let fullWeekCount = businessDays / 7

        if businessDays > fullWeekCount * 7 then
            let mutable firstDayOfWeek = int firstDay.DayOfWeek
            let mutable lastDayOfWeek = int lastDay.DayOfWeek
            if lastDayOfWeek < firstDayOfWeek then
                lastDayOfWeek <- lastDayOfWeek + 7

            if firstDayOfWeek <= 6 then
                if lastDayOfWeek >= 7 then
                    businessDays <- businessDays - 2
                elif lastDayOfWeek >= 6 then
                    businessDays <- businessDays - 1
            elif firstDayOfWeek <= 7 && lastDayOfWeek >= 7 then
                businessDays <- businessDays - 1

        businessDays <- businessDays - (fullWeekCount + fullWeekCount)

        for bh in bankHolidays do
            let bh = bh.Date
            if firstDay <= bh && bh <= lastDay then
                businessDays <- businessDays - 1

        businessDays

    /// EnsureSubscriberEmailOrThrow
    static member EnsureSubscriberEmailOrThrow(email: string) =
        // Explicitly annotate type
        let mutable output: string = CommonHelper.EnsureNotNull(email)
        output <- output.Trim()
        output <- CommonHelper.EnsureMaximumLength(output, 255)
        if not (CommonHelper.IsValidEmail(output)) then
            raise (BlogException("Email is not valid."))
        output

    /// IsValidEmail
    static member IsValidEmail(email: string) =
        if String.IsNullOrEmpty(email) then false
        else emailRegex.IsMatch(email.Trim())

    /// IsValidIpAddress
    static member IsValidIpAddress(ipAddress: string) =
        IPAddress.TryParse(ipAddress, ref Unchecked.defaultof<IPAddress>)

    /// GenerateRandomDigitCode
    static member GenerateRandomDigitCode(length: int) =
        let rnd = Random()
        [ for _ in 1 .. length -> rnd.Next(10).ToString() ] |> String.concat ""

    /// GenerateRandomInteger
    static member GenerateRandomInteger(?min: int, ?max: int) =
        let min = defaultArg min 0
        let max = defaultArg max Int32.MaxValue
        let buffer = Array.zeroCreate<byte> 10
        use rng = new RNGCryptoServiceProvider()
        rng.GetBytes(buffer)
        Random(BitConverter.ToInt32(buffer, 0)).Next(min, max)

    /// EnsureMaximumLength
    static member EnsureMaximumLength(str: string, maxLength: int, ?postfix: string) =
        if String.IsNullOrEmpty(str) then str
        elif str.Length <= maxLength then str
        else
            let pLen = postfix |> Option.map (fun p -> p.Length) |> Option.defaultValue 0
            let result = str.Substring(0, maxLength - pLen)
            match postfix with
            | Some p when not (String.IsNullOrEmpty(p)) -> result + p
            | _ -> result

    /// EnsureNumericOnly
    static member EnsureNumericOnly(str: string) =
        if String.IsNullOrEmpty(str) then ""
        else str |> Seq.filter Char.IsDigit |> Seq.toArray |> String

    /// EnsureNotNull
    static member EnsureNotNull(str: string) =
        if isNull str then "" else str

    /// AreNullOrEmpty
    static member AreNullOrEmpty([<ParamArray>] stringsToValidate: string[]) =
        stringsToValidate |> Array.exists String.IsNullOrEmpty

    /// ArraysEqual
    static member ArraysEqual(a1: 'T[], a2: 'T[]) =
        if Object.ReferenceEquals(a1, a2) then true
        elif isNull a1 || isNull a2 then false
        elif a1.Length <> a2.Length then false
        else
            let comparer = EqualityComparer<'T>.Default
            a1 |> Array.mapi (fun i t -> comparer.Equals(t, a2[i])) |> Array.forall id

    /// SetProperty
    static member SetProperty(instance: obj, propertyName: string, value: obj) =
        if isNull instance then raise (ArgumentNullException(nameof instance))
        if isNull propertyName then raise (ArgumentNullException(nameof propertyName))

        let t = instance.GetType()
        let pi = t.GetProperty(propertyName)
        if isNull pi then
            raise (BlogException($"No property '{propertyName}' found on the instance of type '{t}'."))
        if not pi.CanWrite then
            raise (BlogException($"The property '{propertyName}' on the instance of type '{t}' does not have a setter."))

        let mutable v = value
        if not (isNull v) && not (pi.PropertyType.IsAssignableFrom(v.GetType())) then
            v <- CommonHelper.To(v, pi.PropertyType)

        pi.SetValue(instance, v, [||])

    /// To
    static member To(value: obj, destinationType: Type) =
        CommonHelper.To(value, destinationType, CultureInfo.InvariantCulture)

    static member To(value: obj, destinationType: Type, culture: CultureInfo) =
        if isNull value then null
        else
            let sourceType = value.GetType()
            let destConv = TypeDescriptor.GetConverter(destinationType)
            if destConv.CanConvertFrom(sourceType) then
                destConv.ConvertFrom(null, culture, value)
            else
                let srcConv = TypeDescriptor.GetConverter(sourceType)
                if srcConv.CanConvertTo(destinationType) then
                    srcConv.ConvertTo(null, culture, value, destinationType)
                elif destinationType.IsEnum && value :? int then
                    Enum.ToObject(destinationType, value :?> int)
                elif not (destinationType.IsInstanceOfType(value)) then
                    Convert.ChangeType(value, destinationType, culture)
                else value

    static member To<'T>(value: obj) =
        CommonHelper.To(value, typeof<'T>) :?> 'T

    /// ConvertEnum
    static member ConvertEnum(str: string) =
        if String.IsNullOrEmpty(str) then ""
        else
            str 
            |> Seq.fold (fun acc c ->
                if c.ToString() <> c.ToString().ToLower() then acc + " " + c.ToString()
                else acc + c.ToString()) ""
            |> fun res -> res.TrimStart()

    /// SetTelerikCulture
    static member SetTelerikCulture() =
        let culture = CultureInfo("en-US")
        CultureInfo.CurrentCulture <- culture
        CultureInfo.CurrentUICulture <- culture

    /// GetDifferenceInYears
    static member GetDifferenceInYears(startDate: DateTime, endDate: DateTime) =
        let mutable age = endDate.Year - startDate.Year
        if startDate > endDate.AddYears(-age) then
            age <- age - 1
        age

    /// GetPrivateFieldValue
    static member GetPrivateFieldValue(target: obj, fieldName: string) =
        if isNull target then raise (ArgumentNullException("target", "The assignment target cannot be null."))
        if String.IsNullOrEmpty(fieldName) then raise (ArgumentException("The field name cannot be null or empty.", "fieldName"))

        let mutable t = target.GetType()
        let mutable fi: FieldInfo = null

        while not (isNull t) && isNull fi do
            fi <- t.GetField(fieldName, BindingFlags.Instance ||| BindingFlags.NonPublic)
            t <- t.BaseType

        if isNull fi then raise (Exception($"Field '{fieldName}' not found in type hierarchy."))
        fi.GetValue(target)

