// <copyright file="Email.cs" company="Blog">
// Copyright (c) Blog. All rights reserved.
// </copyright>

namespace Blog.Core.Emails
{
    using System.Collections.Generic;

    /// <summary>
    /// Email.
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Email"/> class.
        /// </summary>
        public Email()
        {
            this.Attachments = new List<byte[]>();
        }

        /// <summary>
        /// Gets or sets body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets attachments.
        /// </summary>
        public List<byte[]> Attachments { get; set; }
    }
}
