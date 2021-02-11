// <copyright file="ApplicationUserDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Dtos.User
{
    using System.Collections.Generic;
    using Blog.Data.Models;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Application user view model.
    /// </summary>
    public class ApplicationUserDto
    {
        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [email confirmed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [email confirmed]; otherwise, <c>false</c>.
        /// </value>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets roles.
        /// </summary>
        public ICollection<IdentityUserRole<string>> Roles { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [phone number confirmed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [phone number confirmed]; otherwise, <c>false</c>.
        /// </value>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        /// <value>
        /// The profile.
        /// </value>
        public Profile Profile { get; set; }
    }
}
