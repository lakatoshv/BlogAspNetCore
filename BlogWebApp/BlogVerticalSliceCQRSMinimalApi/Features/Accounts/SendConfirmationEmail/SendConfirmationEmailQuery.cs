using Blog.Contracts.V1.Responses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.SendConfirmationEmail;

public record SendConfirmationEmailQuery(string Email) : IQuery<BaseResponse>;