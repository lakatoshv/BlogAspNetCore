using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Web.ViewModels.AspNetUser
{
    // TODO: Add Fluent Validations

    /// <summary>
    /// Registration view model
    /// </summary>
    public class RegistrationViewModel
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets normalizedEmail.
        /// </summary>
        public string NormalizedEmail { get; set; }

        /// <summary>
        /// Gets or sets emailConfirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets confirmPassword.
        /// </summary>
        [Required]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets passwordHash.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets firstName.
        /// </summary>
        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets lastName.
        /// </summary>
        [Required]
        [StringLength(64, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets userName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets normalizedUserName.
        /// </summary>
        public string NormalizedUserName { get; set; }

        /// <summary>
        /// Gets or sets phoneNumber.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets phoneNumberConfirmed.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets concurrencyStamp.
        /// </summary>
        public string ConcurrencyStamp { get; set; }

        /// <summary>
        /// Gets or sets twoFactorEnabled.
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets lockoutEnd.
        /// </summary>
        public virtual DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// Gets or sets lockoutEnabled.
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets accessFailedCount.
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Gets or sets securityStamp.
        /// </summary>
        public string SecurityStamp { get; set; }

        // Audit info
        /// <summary>
        /// Gets or sets createdOn.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets modifiedOn.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        /// <summary>
        /// Gets or sets isDeleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets deletedOn.
        /// </summary>
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Gets or sets roles.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// Gets or sets refreshTokens.
        /// </summary>
        public IEnumerable<RefreshToken> RefreshTokens { get; set; }

        /// <summary>
        /// Gets or sets claims.
        /// </summary>
        public IEnumerable<IdentityUserClaim<string>> Claims { get; set; }
    }
}