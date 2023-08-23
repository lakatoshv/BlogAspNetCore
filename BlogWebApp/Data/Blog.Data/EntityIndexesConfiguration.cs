// <copyright file="EntityIndexesConfiguration.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data;

using System.Linq;
using Blog.Data.Core.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Entity indexes configuration.
/// </summary>
internal static class EntityIndexesConfiguration
{
    /// <summary>
    /// Configure.
    /// </summary>
    /// <param name="modelBuilder">modelBuilder.</param>
    public static void Configure(ModelBuilder modelBuilder)
    {
        // IDeletableEntity.IsDeleted index
        var deletableEntityTypes = modelBuilder.Model
            .GetEntityTypes()
            .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
        foreach (var deletableEntityType in deletableEntityTypes)
        {
            modelBuilder.Entity(deletableEntityType.ClrType).HasIndex(nameof(IDeletableEntity.IsDeleted));
        }
    }
}