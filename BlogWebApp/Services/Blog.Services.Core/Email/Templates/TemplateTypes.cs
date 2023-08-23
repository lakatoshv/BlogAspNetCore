// <copyright file="TemplateTypes.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Email.Templates;

/// <summary>
/// Template types.
/// </summary>
public enum TemplateTypes
{
    /// <summary>
    /// Email verification.
    /// </summary>
    EmailVerification = 1,

    /// <summary>
    /// Password restore.
    /// </summary>
    PasswordRestore = 2,
}