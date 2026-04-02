using Blog.Contracts.V1.Responses.Chart;
using Blog.EntityServices.Interfaces;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.PostsActivity;

public class PostsActivityQueryHandler(IPostsService service)
    : IQueryHandler<PostsActivityQuery, ChartDataModel>
{
    public async Task<ChartDataModel> Handle(PostsActivityQuery query, CancellationToken cancellationToken)
        => await service.GetPostsActivity();
}