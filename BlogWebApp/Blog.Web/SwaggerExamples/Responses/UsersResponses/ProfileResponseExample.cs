namespace Blog.Web.SwaggerExamples.Responses.UsersResponses;

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Responses.UsersResponses;
using Core.Consts;

/// <summary>
/// Profile response example.
/// </summary>
/// <seealso cref="IExamplesProvider{ProfileResponse}" />
public class ProfileResponseExample : IExamplesProvider<ProfileResponse>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public ProfileResponse GetExamples()
    {
        return new ProfileResponse
        {
            UserId = Guid.NewGuid().ToString(),

            User = new ApplicationUserResponse
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
            },

            About = SwaggerExamplesConsts.ProfileResponseExample.About,
            ProfileImg = SwaggerExamplesConsts.ProfileResponseExample.ProfileImg
        };
    }
}