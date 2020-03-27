// <copyright file="Invitation.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Email.Invitation
{
    /// <summary>
    /// Invitation model.
    /// </summary>
    public class Invitation
    {
        /// <summary>
        /// Gets or sets inviterEmail.
        /// </summary>
        public string InviterEmail { get; set; }

        /// <summary>
        /// Gets or sets inviterName.
        /// </summary>
        public string InviterName { get; set; }

        /// <summary>
        /// Gets or sets securityStamp.
        /// </summary>
        public string SecurityStamp { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }
    }
}
