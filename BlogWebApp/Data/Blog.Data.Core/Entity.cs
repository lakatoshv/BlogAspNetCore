// <copyright file="Entity.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data.Core;

using System;
using Blog.Core;

/// <summary>
/// Entity.
/// </summary>
public abstract class Entity : IEntityBase<int>
{
    /// <summary>
    /// Gets or sets id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Override ==.
    /// </summary>
    /// <param name="x">x.</param>
    /// <param name="y">y.</param>
    /// <returns>bool.</returns>
    public static bool operator ==(Entity x, Entity y)
    {
        return Equals(x, y);
    }

    /// <summary>
    /// Override !=.
    /// </summary>
    /// <param name="x">x.</param>
    /// <param name="y">y.</param>
    /// <returns>bool.</returns>
    public static bool operator !=(Entity x, Entity y)
    {
        return !(x == y);
    }

    /// <summary>
    /// Equals.
    /// </summary>
    /// <param name="obj">obj.</param>
    /// <returns>bool.</returns>
    public override bool Equals(object obj)
    {
        return this.Equals(obj as Entity);
    }

    /// <summary>
    /// Equals.
    /// </summary>
    /// <param name="other">other.</param>
    /// <returns>bool.</returns>
    public virtual bool Equals(Entity other)
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
    private static bool IsTransient(Entity obj)
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