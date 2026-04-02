using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.Login;

public record LoginCommand(string Email, string Password)
    : ICommand<string>;