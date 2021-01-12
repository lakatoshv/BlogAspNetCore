namespace AuthService.Data.Core.Models
{
    using Interfaces;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Base model.
    /// </summary>
    /// <typeparam name="TKey">TKey.</typeparam>
    public abstract class BaseModel<TKey> : IAuditInfo
    {
        protected BaseModel(TKey id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        [Key]
        public TKey Id { get; }

        /// <summary>
        /// Gets or sets created on.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets modified on.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
    }
}
