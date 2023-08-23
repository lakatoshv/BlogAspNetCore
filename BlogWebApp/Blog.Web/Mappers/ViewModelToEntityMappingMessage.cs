namespace Blog.Web.Mappers;

using Profile = AutoMapper.Profile;
using Data.Models;
using Contracts.V1.Requests.MessagesRequests;
using Contracts.V1.Responses;

/// <summary>
/// View model to entity mapping message.
/// </summary>
/// <seealso cref="Profile" />
public class ViewModelToEntityMappingMessage : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelToEntityMappingMessage"/> class.
    /// </summary>
    public ViewModelToEntityMappingMessage()
    {
        CreateMap<CreateMessageRequest, Message>();
        CreateMap<UpdateMessageRequest, Message>();
        CreateMap<Message, MessageResponse>();
    }
}