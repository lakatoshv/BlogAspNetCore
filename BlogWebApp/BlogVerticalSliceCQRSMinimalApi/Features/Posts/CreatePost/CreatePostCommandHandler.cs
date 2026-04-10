using AutoMapper;
using Blog.Contracts.V1.Responses;
using Blog.Data.Models;
using Blog.EntityServices.Interfaces;
using BlogVerticalSliceCQRSMinimalApi.CQRSExtensions;

namespace BlogVerticalSliceCQRSMinimalApi.Features.Posts.CreatePost;

public class GetPostsCommandHandler(IPostsService service, IMapper mapper)
    : ICommandHandler<GetPostsCommand, CreatedResponse<int>>
{
    public async Task<CreatedResponse<int>> Handle(GetPostsCommand command, CancellationToken cancellationToken)
    {
        var post = new Post
        {
            Title = command.Title,
            Description = command.Description,
            Content = command.Content,
            ImageUrl = command.ImageUrl,
            AuthorId = command.AuthorId,
            CreatedAt = command.CreatedAt
        };

        var tags = mapper.Map<List<Tag>>(command.Tags.Distinct());

        await service.InsertAsync(post, tags);

        return new CreatedResponse<int>
        {
            Id = post.Id
        };
    }
}
