namespace Blog.Web.SwaggerExamples.Responses.UsersResponses;

using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses.UsersResponses;
using Core.Consts;

/// <summary>
/// Account response example.
/// </summary>
/// <seealso cref="IExamplesProvider{AccountResponse}" />
public class AccountResponseExample : IExamplesProvider<AccountResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public AccountResponse GetExamples()
    {
        return new AccountResponse
        {
            Id = Guid.NewGuid(),
            FirstName = SwaggerExamplesConsts.AccountExample.FirstName,
            LastName = SwaggerExamplesConsts.AccountExample.LastName,
            UserName = SwaggerExamplesConsts.AccountExample.UserName,
            Email = SwaggerExamplesConsts.AccountExample.Email,
            PhoneNumber = SwaggerExamplesConsts.AccountExample.PhoneNumber,
            Roles = new List<RoleResponse>
            {
                new RoleResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Consts.Roles.User,
                },
            }
        };
    }
}