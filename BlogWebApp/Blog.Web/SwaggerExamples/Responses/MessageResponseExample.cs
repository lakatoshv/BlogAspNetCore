namespace Blog.Web.SwaggerExamples.Responses;

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses;
using Blog.Contracts.V1.Responses.UsersResponses;
using Blog.Core.Consts;
using Core.Enums;

/// <summary>
/// Message response example.
/// </summary>
/// <seealso cref="IExamplesProvider{MessageResponse}" />
public class MessageResponseExample : IExamplesProvider<MessageResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public MessageResponse GetExamples()
    {
        return new MessageResponse
        {
            SenderId = Guid.NewGuid().ToString(),
            Sender = new ApplicationUserResponse
            {
                FirstName = SwaggerExamplesConsts.AccountExample.FirstName,
                LastName = SwaggerExamplesConsts.AccountExample.LastName,
                Email = SwaggerExamplesConsts.AccountExample.Email,
                EmailConfirmed = true,
                Roles = new List<IdentityUserRole<string>>
                {
                    new IdentityUserRole<string>
                    {
                        RoleId = Guid.NewGuid().ToString(),
                    }
                }
            },

            RecipientId = Guid.NewGuid().ToString(),
            Recipient = new ApplicationUserResponse
            {
                FirstName = SwaggerExamplesConsts.AccountExample.FirstName,
                LastName = SwaggerExamplesConsts.AccountExample.LastName,
                Email = SwaggerExamplesConsts.AccountExample.Email,
                EmailConfirmed = true,
                Roles = new List<IdentityUserRole<string>>
                {
                    new IdentityUserRole<string>
                    {
                        RoleId = Guid.NewGuid().ToString(),
                    }
                }
            },

            Subject = SwaggerExamplesConsts.MessageResponseExample.Subject,
            Body = SwaggerExamplesConsts.MessageResponseExample.Body,
            MessageType = MessageType.MessageFoAdmins,
        };
    }
}