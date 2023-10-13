namespace Blog.Web.SwaggerExamples.Responses.UsersResponses;

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses.UsersResponses;
using Core.Consts;

/// <summary>
/// Application user response example.
/// </summary>
/// <seealso cref="IExamplesProvider{ApplicationUserResponse}" />
public class ApplicationUserResponseExample : IExamplesProvider<ApplicationUserResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public ApplicationUserResponse GetExamples()
    {
        return new ApplicationUserResponse
        {
            FirstName = SwaggerExamplesConsts.AccountExample.FirstName,
            LastName = SwaggerExamplesConsts.AccountExample.LastName,
            Email = SwaggerExamplesConsts.AccountExample.Email,
            EmailConfirmed = true,
            Roles = new List<IdentityUserRole<string>>
            {
                new ()
                {
                    RoleId = Guid.NewGuid().ToString()
                }
            }
        };
    }
}