namespace Blog.FSharp.Core

/// <summary>
/// Entity interface.
/// </summary>
type IEntity = interface end

/// <summary>
/// Entity base interface.
/// </summary>
type IEntityBase =
    inherit IEntity

/// <summary>
/// Entity interface.
/// </summary>
/// <typeparam name="T">Type.</typeparam>
type IEntity<'T> =
    inherit IEntity
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    abstract member Id: 'T with get, set

/// <summary>
/// Entity base interface.
/// </summary>
/// <typeparam name="T">Type.</typeparam>
type IEntityBase<'T> =
    inherit IEntityBase
    inherit IEntity<'T>
