// <copyright file="IActiveDirectory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.CommonServices.Interfaces;

using System.Collections.Generic;
using Blog.Services.Core.Dtos.User;

public interface IActiveDirectoryService
{
    ActiveDirectoryUserDto GetActiveDirectoryUserByIdentity(string identity);
    ActiveDirectoryUserDto GetActiveDirectoryUserByEmployeeId(string employeeId);
    List<ActiveDirectoryUserDto> GetActiveDirectoryUsersByGroup(string groupName);
}