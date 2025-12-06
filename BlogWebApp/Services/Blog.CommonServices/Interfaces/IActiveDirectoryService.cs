// <copyright file="IActiveDirectory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.CommonServices.Interfaces;

using Blog.Services.Core.Dtos.User;
using System.Collections.Generic;

/// <summary>
/// The Active Directory service interface.
/// </summary>
public interface IActiveDirectoryService
{
    /// <summary>
    /// Gets Active Directory User by identity.
    /// </summary>
    /// <param name="identity">The identity.</param>
    /// <returns>ActiveDirectoryUserDto.</returns>
    ActiveDirectoryUserDto GetActiveDirectoryUserByIdentity(string identity);

    /// <summary>
    /// Gets Active Directory User by employee id.
    /// </summary>
    /// <param name="employeeId">The employee id.</param>
    /// <returns>ActiveDirectoryUserDto.</returns>
    ActiveDirectoryUserDto GetActiveDirectoryUserByEmployeeId(string employeeId);

    /// <summary>
    /// Gets Active Directory Users by group.
    /// </summary>
    /// <param name="groupName">The group name.</param>
    /// <returns>ActiveDirectoryUserDto.</returns>
    List<ActiveDirectoryUserDto> GetActiveDirectoryUsersByGroup(string groupName);
}