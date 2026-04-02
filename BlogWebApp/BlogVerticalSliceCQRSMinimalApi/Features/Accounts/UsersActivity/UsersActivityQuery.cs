using Blog.Contracts.V1.Responses.Chart;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Accounts.UsersActivity;

public class UsersActivityQuery : IQuery<ChartDataModel>;