using AutoMapper;
using Blog.Contracts.V1.Responses.UsersResponses;
using Blog.Data.Models;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.Initialize;

public class InitializeQueryHandler(
    RoleManager<ApplicationRole> roleManager,
    IMapper mapper)
    : IQueryHandler<InitializeQuery, List<RoleResponse>>
{
    public async Task<List<RoleResponse>> Handle(InitializeQuery query, CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles.ToListAsync(cancellationToken: cancellationToken);

        return mapper.Map<List<RoleResponse>>(roles);
    }
}