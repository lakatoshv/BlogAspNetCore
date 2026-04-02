using Blog.Contracts.V1.Responses.Chart;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.PostsActivity;

public record PostsActivityQuery : IQuery<ChartDataModel>;