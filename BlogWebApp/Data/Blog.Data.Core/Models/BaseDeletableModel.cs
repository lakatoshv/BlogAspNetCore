// <copyright file="BaseDeletableModel.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Core.Models
{
    using System;
    using Blog.Core;
    using Interfaces;

    /// <summary>
    /// Base deletable model.
    /// </summary>
    /// <typeparam name="TKey">TKey.</typeparam>
    public abstract class BaseDeletableModel<TKey> : BaseModel<TKey>, IDeletableEntity, IEntity
    {
        /// <summary>
        /// Gets or sets a value indicating whether is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets deleted on.
        /// </summary>
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Override ==.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <returns>bool.</returns>
        public static bool operator ==(BaseDeletableModel<TKey> x, BaseDeletableModel<TKey> y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// OVerride !=.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <returns>bool.</returns>
        public static bool operator !=(BaseDeletableModel<TKey> x, BaseDeletableModel<TKey> y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDeletableModel{TKey}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        protected BaseDeletableModel(TKey id) : base(id)
        {
        }

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="obj">obj.</param>
        /// <returns>bool.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as BaseDeletableModel<TKey>);
        }

        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="other">other.</param>
        /// <returns>bool.</returns>
        public virtual bool Equals(BaseDeletableModel<TKey> other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (IsTransient(this) || IsTransient(other) || !Equals(this.Id, other.Id))
            {
                return false;
            }

            var otherType = other.GetUnproxiedType();
            var thisType = this.GetUnproxiedType();

            return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
        }

        /// <inheritdoc cref="object"/>
        public override int GetHashCode()
        {
            return Equals(this.Id, default(int)) ? base.GetHashCode() : this.Id.GetHashCode();
        }

        /// <summary>
        /// Is transient.
        /// </summary>
        /// <param name="obj">obj.</param>
        /// <returns>bool.</returns>
        private static bool IsTransient(BaseDeletableModel<TKey> obj)
        {
            return obj != null && Equals(obj.Id, default(int));
        }

        /// <summary>
        /// Get unproxied type.
        /// </summary>
        /// <returns>Type.</returns>
        private Type GetUnproxiedType()
        {
            return this.GetType();
        }
    }
}
