using Blog.CommonServices.EmailServices.Interfaces;
using Blog.Contracts.V1.Responses;
using Blog.EntityServices.EntityFrameworkServices.Identity.User;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.SendConfirmationEmail;

public class SendConfirmationEmailQueryHandler(
    IUserService userService,
    IEmailExtensionService emailService)
    : IQueryHandler<SendConfirmationEmailQuery, BaseResponse>
{
    public async Task<BaseResponse> Handle(SendConfirmationEmailQuery query, CancellationToken cancellationToken)
    {
        var token = await userService.GetEmailVerificationTokenAsync(query.Email);
        await emailService.SendVerificationEmailAsync(query.Email, token);

        return new BaseResponse();
    }
}