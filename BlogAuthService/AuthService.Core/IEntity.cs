namespace AuthService.Core
{
    /// <summary>
    /// Entity interface.
    /// </summary>
    public interface IEntity
    {
    }

    /// <summary>
    /// Entity base interface.
    /// </summary>
    public interface IEntityBase : IEntity
    {
    }

    /// <summary>
    /// Entity interface.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public interface IEntity<T> : IEntity
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        T Id { get; set; }
    }

    /// <summary>
    /// Entity base interface.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    public interface IEntityBase<T> : IEntityBase
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        T Id { get; set; }
    }
}
