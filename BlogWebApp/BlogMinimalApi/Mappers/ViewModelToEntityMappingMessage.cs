namespace BlogMinimalApi.Mappers;

using Profile = AutoMapper.Profile;
using Blog.Data.Models;
using Blog.Contracts.V1.Requests.MessagesRequests;
using Blog.Contracts.V1.Responses;

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