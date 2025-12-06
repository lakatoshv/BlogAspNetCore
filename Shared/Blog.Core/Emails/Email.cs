// <copyright file="Email.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Core.Emails;

using System.Collections.Generic;

/// <summary>
/// Email.
/// </summary>
public class Email
{
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
    public List<byte[]> Attachments { get; set; } = [];
}