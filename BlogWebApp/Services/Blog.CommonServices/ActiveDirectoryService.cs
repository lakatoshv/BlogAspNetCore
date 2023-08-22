using System.Collections.Generic;
using Blog.CommonServices.Interfaces;
using Blog.Services.Core.Dtos.User;

namespace Blog.CommonServices;

public class ActiveDirectoryService : IActiveDirectoryService
{
    public ActiveDirectoryUserDto GetActiveDirectoryUserByIdentity(string identity)
    {
        throw new System.NotImplementedException();
    }

    public ActiveDirectoryUserDto GetActiveDirectoryUserByEmployeeId(string employeeId)
    {
        throw new System.NotImplementedException();
    }

    public List<ActiveDirectoryUserDto> GetActiveDirectoryUsersByGroup(string groupName)
    {
        throw new System.NotImplementedException();
    }
}