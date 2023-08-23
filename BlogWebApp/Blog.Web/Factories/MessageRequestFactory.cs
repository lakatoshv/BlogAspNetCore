namespace Blog.Web.Factories;

using AutoMapper;
using System;
using Calabonga.Microservices.Core.Exceptions;
using Core.Enums;
using Contracts.V1.Requests.MessagesRequests;
using Data;
using Data.Models;
using Data.Specifications;
using Data.UnitOfWork;
using Base;

/// <summary>
/// Message request factory.
/// </summary>
/// <seealso cref="MessageRequestFactory" />
public class MessageRequestFactory : RequestFactory<int, Message, CreateMessageRequest, UpdateMessageRequest>
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
    public MessageRequestFactory(
        IMapper mapper,
        IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc cref="RequestFactory{T,TEntity,TCreateRequest,TUpdateRequest}"/>
    public override CreateMessageRequest GenerateForCreate() =>
        new () {
            RecipientId = Guid.NewGuid().ToString(),
            SenderEmail = "sender@gmail.com",
            SenderName = "sender@gmail.com",
            Subject = "Some email",
            Body = "Some email body.",
            MessageType = (int)MessageType.MessageFoAdmins,
        };

    /// <inheritdoc cref="RequestFactory{T,TEntity,TCreateRequest,TUpdateRequest}"/>
    public override UpdateMessageRequest GenerateForUpdate(int id)
    {
        var message = _unitOfWork.GetRepository<Message>().FirstOrDefault(new MessageSpecification(x => x.Id == id));
        if (message == null)
        {
            throw new MicroserviceArgumentNullException();
        }

        var mapped = _mapper.Map<UpdateMessageRequest>(message);

        return mapped;
    }
}