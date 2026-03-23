using Blog.Contracts.V1.Responses.PostsResponses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.HealthCheck.Initialize;

public class InitializeQueryHandler
    : IQueryHandler<InitializeQuery, PostWithPagedCommentsResponse>
{
    public Task<PostWithPagedCommentsResponse> Handle(InitializeQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}