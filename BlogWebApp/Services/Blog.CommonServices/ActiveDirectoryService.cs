using Blog.CommonServices.Interfaces;
using Blog.Services.Core.Dtos.User;
using System.Collections.Generic;

namespace Blog.CommonServices;

/// <summary>
/// Active Directory service.
/// </summary>
/// <seealso cref="IActiveDirectoryService" />
public class ActiveDirectoryService : IActiveDirectoryService
{
    /// <inheritdoc cref="IActiveDirectoryService"/>
    public ActiveDirectoryUserDto GetActiveDirectoryUserByIdentity(string identity)
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc cref="IActiveDirectoryService"/>
    public ActiveDirectoryUserDto GetActiveDirectoryUserByEmployeeId(string employeeId)
    {
        throw new System.NotImplementedException();
    }

    /// <inheritdoc cref="IActiveDirectoryService"/>
    public List<ActiveDirectoryUserDto> GetActiveDirectoryUsersByGroup(string groupName)
    {
        throw new System.NotImplementedException();
    }
}