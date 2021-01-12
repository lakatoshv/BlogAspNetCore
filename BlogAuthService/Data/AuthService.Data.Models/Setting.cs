namespace AuthService.Data.Models
{
    using Core;

    /// <summary>
    /// Setting.
    /// </summary>
    /// <seealso cref="Entity" />
    public class Setting : Entity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
    }
}
