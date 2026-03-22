using Blog.Contracts.V1.Responses.Chart;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.CommentsActivity;

public record CommentsActivityQuery : IQuery<ChartDataModel>;