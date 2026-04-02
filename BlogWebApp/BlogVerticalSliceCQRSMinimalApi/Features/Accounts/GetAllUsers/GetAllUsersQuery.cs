using Blog.Contracts.V1.Responses.UsersResponses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.GetAllUsers;

public record GetAllUsersQuery : IQuery<List<AccountResponse>>;