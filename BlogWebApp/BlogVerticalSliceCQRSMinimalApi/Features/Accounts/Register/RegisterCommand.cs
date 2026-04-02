using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;
using Microsoft.AspNetCore.Identity;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.Register;

public record RegisterCommand(string Email, string Password, string ConfirmPassword, string FirstName,
    string LastName, string UserName, string PhoneNumber,string ConcurrencyStamp, IEnumerable<string> Roles)
    : ICommand<IdentityResult>;
