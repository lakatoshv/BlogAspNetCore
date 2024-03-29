﻿namespace Blog.Web.Factories;

using AutoMapper;
using Calabonga.Microservices.Core.Exceptions;
using Contracts.V1.Requests.TagsRequests;
using Data;
using Data.Models;
using Data.UnitOfWork;
using Data.Specifications;
using Base;

/// <summary>
/// Tag request factory.
/// </summary>
/// <seealso cref="UpdateTagRequest" />
public class TagRequestFactory : RequestFactoryWithSample<int, Tag, TagRequest, CreateTagRequest, UpdateTagRequest>
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
    public TagRequestFactory(
        IMapper mapper,
        IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc cref="RequestFactoryWithSample{T,TEntity,TSampleRequest,TCreateRequest,TUpdateRequest}"/>
    public override CreateTagRequest GenerateForCreate() =>
        new()
        {
            Title = "Create Tag from factory"
        };

    /// <inheritdoc cref="RequestFactoryWithSample{T,TEntity,TSampleRequest,TCreateRequest,TUpdateRequest}"/>
    public override UpdateTagRequest GenerateForUpdate(int id)
    {
        var tag = _unitOfWork.GetRepository<Tag>().FirstOrDefault(new TagSpecification(x => x.Id == id));
        if (tag == null)
        {
            throw new MicroserviceArgumentNullException();
        }

        var mapped = _mapper.Map<UpdateTagRequest>(tag);

        return mapped;
    }

    /// <inheritdoc cref="RequestFactoryWithSample{T,TEntity,TSampleRequest,TCreateRequest,TUpdateRequest}"/>
    public override TagRequest GenerateForSampleRequest() =>
        new TagRequest()
        {
            Title = "Tag",
        };
}