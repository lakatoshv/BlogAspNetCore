using Blog.Data.Models;
using Blog.Web.Contracts.V1.Requests.MessagesRequests;
using Blog.Web.Contracts.V1.Responses;
using Profile = AutoMapper.Profile;

namespace Blog.Web.Mappers
{
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
}