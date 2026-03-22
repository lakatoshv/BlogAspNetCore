using Blog.Contracts.V1.Responses.CommentsResponses;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Comments.GetAll;

public record GetAllQuery : IQuery<List<CommentResponse>>;