using Blog.EntityServices.EntityFrameworkServices.Identity.Auth;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.Login;

public class LoginCommandHandler(IAuthService authService)
    : ICommandHandler<LoginCommand, string>
{
    public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
        => await authService.GetJwtAsync(command.Email, command.Password);
}