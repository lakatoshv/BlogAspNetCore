// <copyright file="ApplicationDbContext.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Data;

using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Blog.Data.Core.Models.Interfaces;
using Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Application database context.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    /// <summary>
    /// Set is deleted query filter method.
    /// </summary>
    private static readonly MethodInfo SetIsDeletedQueryFilterMethod =
        typeof(ApplicationDbContext).GetMethod(
            nameof(SetIsDeletedQueryFilter),
            BindingFlags.NonPublic | BindingFlags.Static);

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">options.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets settings.
    /// </summary>
    public DbSet<Setting> Settings { get; set; }

    /// <summary>
    /// Gets or sets refreshTokens.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    /// <summary>
    /// Gets or sets posts.
    /// </summary>
    public DbSet<Post> Posts { get; set; }

    /// <summary>
    /// Gets or sets the comments.
    /// </summary>
    /// <value>
    /// The comments.
    /// </value>
    public DbSet<Comment> Comments { get; set; }

    /// <summary>
    /// Gets or sets the profiles.
    /// </summary>
    /// <value>
    /// The profiles.
    /// </value>
    public DbSet<Profile> Profiles { get; set; }

    /// <summary>
    /// Gets or sets the messages.
    /// </summary>
    /// <value>
    /// The messages.
    /// </value>
    public DbSet<Message> Messages { get; set; }

    /// <summary>
    /// Gets or sets the tags.
    /// </summary>
    /// <value>
    /// The tags.
    /// </value>
    public DbSet<Tag> Tags { get; set; }

    /// <summary>
    /// Gets or sets the posts tags relations.
    /// </summary>
    /// <value>
    /// The posts tags relations.
    /// </value>
    public DbSet<PostsTagsRelations> PostsTagsRelations { get; set; }

    /// <summary>
    /// Gets or sets the categories.
    /// </summary>
    /// <value>
    /// The categories.
    /// </value>
    public DbSet<Category> Categories { get; set; }

    /// <summary>
    /// Save changes.
    /// </summary>
    /// <returns>int.</returns>
    public override int SaveChanges() => this.SaveChanges(true);

    /// <summary>
    /// Save changes.
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">acceptAllChangesOnSuccess.</param>
    /// <returns>int.</returns>
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.ApplyAuditInfoRules();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    /// <summary>
    /// Async save changes.
    /// </summary>
    /// <param name="cancellationToken">cancellationToken.</param>
    /// <returns>Task.</returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        this.SaveChangesAsync(true, cancellationToken);

    /// <summary>
    /// Async save changes.
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">acceptAllChangesOnSuccess.</param>
    /// <param name="cancellationToken">cancellationToken.</param>
    /// <returns>Task.</returns>
    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        this.ApplyAuditInfoRules();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <summary>
    /// On model creating.
    /// </summary>
    /// <param name="builder">builder.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Needed for Identity models configuration
        base.OnModelCreating(builder);

        ConfigureUserIdentityRelations(builder);

        EntityIndexesConfiguration.Configure(builder);

        var entityTypes = builder.Model.GetEntityTypes().ToList();

        // Set global query filter for not deleted entities only
        var deletableEntityTypes = entityTypes
            .Where(et => et.ClrType != null && typeof(IDeletableEntity).IsAssignableFrom(et.ClrType));
        foreach (var deletableEntityType in deletableEntityTypes)
        {
            var method = SetIsDeletedQueryFilterMethod.MakeGenericMethod(deletableEntityType.ClrType);
            method.Invoke(null, new object[] { builder });
        }

        // Disable cascade delete
        var foreignKeys = entityTypes
            .SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));
        foreach (var foreignKey in foreignKeys)
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }

        builder.Entity<Comment>()
            .HasOne(p => p.Post)
            .WithMany(t => t.Comments)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Profile>()
            .HasOne(p => p.User)
            .WithOne(t => t.Profile)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ApplicationUser>()
            .HasMany(p => p.Posts)
            .WithOne(t => t.Author)
            .HasForeignKey(x => x.AuthorId);

        builder.Entity<ApplicationUser>()
            .HasMany(p => p.SentMessages)
            .WithOne(t => t.Sender)
            .HasForeignKey(x => x.SenderId);

        builder.Entity<ApplicationUser>()
            .HasMany(p => p.ReceivedMessages)
            .WithOne(t => t.Recipient)
            .HasForeignKey(x => x.RecipientId);

        builder.Entity<Post>()
            .HasOne(bc => bc.Category)
            .WithMany(b => b.Posts)
            .HasForeignKey(bc => bc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<PostsTagsRelations>()
            .HasKey(x => new { x.PostId, x.TagId });
        builder.Entity<PostsTagsRelations>()
            .HasOne(bc => bc.Tag)
            .WithMany(b => b.PostsTagsRelations)
            .HasForeignKey(bc => bc.TagId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<PostsTagsRelations>()
            .HasOne(bc => bc.Post)
            .WithMany(c => c.PostsTagsRelations)
            .HasForeignKey(bc => bc.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>
    /// Configure user identity relations.
    /// </summary>
    /// <param name="builder">builder.</param>
    private static void ConfigureUserIdentityRelations(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>()
            .HasMany(e => e.Claims)
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ApplicationUser>()
            .HasMany(e => e.Logins)
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ApplicationUser>()
            .HasMany(e => e.Roles)
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    /// Set is deleted query filter.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="builder">builder.</param>
    private static void SetIsDeletedQueryFilter<T>(ModelBuilder builder)
        where T : class, IDeletableEntity
    {
        builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
    }

    /// <summary>
    /// Apply audit info rules.
    /// </summary>
    private void ApplyAuditInfoRules()
    {
        var changedEntries = this.ChangeTracker
            .Entries()
            .Where(e =>
                e.Entity is IAuditInfo &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in changedEntries)
        {
            var entity = (IAuditInfo)entry.Entity;
            if (entry.State == EntityState.Added && entity.CreatedOn == default)
            {
                entity.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                entity.ModifiedOn = DateTime.UtcNow;
            }
        }
    }
}