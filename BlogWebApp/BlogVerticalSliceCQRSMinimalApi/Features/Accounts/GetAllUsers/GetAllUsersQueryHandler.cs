using AutoMapper;
using Blog.Contracts.V1.Responses.UsersResponses;
using Blog.Data.Models;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.GetAllUsers;

public class GetAllUsersQueryHandler(
    UserManager<ApplicationUser> userManager,
    IMapper mapper)
    : IQueryHandler<GetAllUsersQuery, List<AccountResponse>>
{
    public async Task<List<AccountResponse>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await userManager.Users
            .Include(u => u.Roles)
            .ToListAsync(cancellationToken: cancellationToken);

        return mapper.Map<List<AccountResponse>>(users);
    }
}