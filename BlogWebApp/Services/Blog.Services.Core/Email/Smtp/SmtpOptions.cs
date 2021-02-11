// <copyright file="SmtpOptions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Email.Smtp
{
    /// <summary>
    /// Smtp options.
    /// </summary>
    public class SmtpOptions
    {
        /// <summary>
        /// Gets or sets host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets userName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enableSsl.
        /// </summary>
        public bool EnableSsl { get; set; }
    }
}
