namespace Blog.FSharpData.Core

open System
open Blog.FSharp.Core

[<AbstractClass>]
type Entity() =
    let mutable id = 0

    interface IEntityBase<int> with
        member this.Id
            with get() = id
            and set(value) = id <- value

    
    /// <summary>
    /// Equals.
    /// </summary>
    /// <param name="obj">obj.</param>
    /// <returns>bool.</returns>
    override this.Equals(obj: obj) =
        match obj with
        | :? Entity as other -> this.Equals(other)
        | _ -> false

    /// <summary>
    /// Equals.
    /// </summary>
    /// <param name="other">other.</param>
    /// <returns>bool.</returns>
    member this.Equals(other: Entity) =
        if isNull (box other) then false
        elif Object.ReferenceEquals(this, other) then true
        elif Entity.IsTransient(this) || Entity.IsTransient(other) || (this.Id <> other.Id) then false
        else
            let thisType: Type = this.GetUnproxiedType()
            let otherType: Type = other.GetUnproxiedType()

            thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType)

    /// <inheritdoc cref="object"/>
    override this.GetHashCode() =
        match this.Id with
        | 0 -> System.Object().GetHashCode()
        | _ -> this.Id.GetHashCode()

    
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    member this.Id
        with get() =
            (this :> IEntityBase<int>).Id
        and set(v) =
            (this :> IEntityBase<int>).Id <- v

    /// <summary>
    /// Get unproxied type.
    /// </summary>
    /// <returns>Type.</returns>
    member private this.GetUnproxiedType() =
        this.GetType()

    /// <summary>
    /// Is transient.
    /// </summary>
    /// <param name="obj">obj.</param>
    /// <returns>bool.</returns>
    static member private IsTransient(e: Entity) =
        not (isNull (box e)) && e.Id = 0

    /// <summary>
    /// Override ==.
    /// </summary>
    /// <param name="x">x.</param>
    /// <param name="y">y.</param>
    /// <returns>bool.</returns>
    static member op_Equality(left: Entity, right: Entity) =
        Object.Equals(left, right)

    /// <summary>
    /// Override !=.
    /// </summary>
    /// <param name="x">x.</param>
    /// <param name="y">y.</param>
    /// <returns>bool.</returns>
    static member op_Inequality(left: Entity, right: Entity) =
        not (Object.Equals(left, right))
