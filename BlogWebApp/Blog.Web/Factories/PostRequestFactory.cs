namespace Blog.Web.Factories;

using System;
using AutoMapper;
using Calabonga.Microservices.Core.Exceptions;
using Contracts.V1.Requests.PostsRequests;
using Data;
using Data.Models;
using Data.UnitOfWork;
using Data.Specifications;
using Base;

/// <summary>
/// Post request factory.
/// </summary>
/// <seealso cref="UpdatePostRequest" />
public class PostRequestFactory : RequestFactoryWithSearchParameters<int, Post, PostsSearchParametersRequest, CreatePostRequest, UpdatePostRequest>
{
    /// <summary>
    /// The mapper.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// The unit of work.
    /// </summary>
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageRequestFactory"/> class.
    /// </summary>
    /// <param name="mapper">The mapper.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public PostRequestFactory(
        IMapper mapper,
        IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc cref="RequestFactoryWithSearchParameters{T,TEntity,TSearchParametersRequest,TCreateRequest,TUpdateRequest}"/>
    public override CreatePostRequest GenerateForCreate() =>
        new ()
        {
            Title =  "Post from factory",
            Description = "Post from factory",
            Content = "Post from factory",
            ImageUrl = "url",
            AuthorId = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.Now
        };

    /// <inheritdoc cref="RequestFactoryWithSearchParameters{T,TEntity,TSearchParametersRequest,TCreateRequest,TUpdateRequest}"/>
    public override UpdatePostRequest GenerateForUpdate(int id)
    {
        var post = _unitOfWork.GetRepository<Post>().FirstOrDefault(new PostSpecification(x => x.Id == id))
            ?? throw new MicroserviceArgumentNullException();

        var mapped = _mapper.Map<UpdatePostRequest>(post);

        return mapped;
    }

    /// <inheritdoc cref="RequestFactoryWithSearchParameters{T,TEntity,TSearchParametersRequest,TCreateRequest,TUpdateRequest}"/>
    public override PostsSearchParametersRequest GenerateForSearchParametersRequest()
    {
        var tag = _unitOfWork.GetRepository<Tag>().FirstOrDefault(null)
            ?? throw new MicroserviceArgumentNullException();

        return new PostsSearchParametersRequest
        {
            Tag = tag.Title,
        };
    }
}