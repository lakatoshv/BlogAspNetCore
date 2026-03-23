using Blog.Contracts.V1.Responses.PostsResponses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.HealthCheck.Initialize;

public class InitializeQuery : IQuery<PostWithPagedCommentsResponse>;