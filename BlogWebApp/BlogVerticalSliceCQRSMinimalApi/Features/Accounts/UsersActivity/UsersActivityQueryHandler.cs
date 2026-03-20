using Blog.Contracts.V1.Responses.Chart;
using Blog.EntityServices.EntityFrameworkServices.Identity.User;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.UsersActivity;

public class UsersActivityQueryHandler(IUserService userService)
    : IQueryHandler<UsersActivityQuery, ChartDataModel>
{
    public async Task<ChartDataModel> Handle(UsersActivityQuery request, CancellationToken cancellationToken)
        => await userService.GetUsersActivity();
}