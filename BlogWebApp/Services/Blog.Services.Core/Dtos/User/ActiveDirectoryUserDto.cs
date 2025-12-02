// <copyright file="ActiveDirectoryUserDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Dtos.User;

using System.Collections.Generic;

/// <summary>
/// The Active directory user dto.
/// </summary>
public class ActiveDirectoryUserDto
{
    /// <summary>
    /// The gets or sets first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// The gets or sets last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// The gets or sets email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The gets or sets identity.
    /// </summary>
    public string Identity { get; set; }

    /// <summary>
    /// The gets or sets display name.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// The gets or sets groups.
    /// </summary>
    public List<string> Groups { get; set; } = [];
}