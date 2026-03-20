using Blog.Contracts.V1.Responses.UsersResponses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.Initialize;

public record InitializeQuery(string UserId) : IQuery<List<RoleResponse>>;