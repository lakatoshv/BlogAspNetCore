using System.Collections.Generic;

namespace Blog.Services.Core.Dtos.User;

public class ActiveDirectoryUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Identity { get; set; }
    public string DisplayName { get; set; }
    public List<string> Groups { get; set; } = [];
}