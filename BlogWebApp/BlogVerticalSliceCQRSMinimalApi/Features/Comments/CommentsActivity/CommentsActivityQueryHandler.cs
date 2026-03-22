using Blog.Contracts.V1.Responses.Chart;
using Blog.EntityServices.Interfaces;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.CommentsActivity;

public class CommentsActivityQueryHandler(ICommentsService service)
    : IQueryHandler<CommentsActivityQuery, ChartDataModel>
{
    public async Task<ChartDataModel> Handle(CommentsActivityQuery query, CancellationToken cancellationToken)
        => await service.GetCommentsActivity();
}