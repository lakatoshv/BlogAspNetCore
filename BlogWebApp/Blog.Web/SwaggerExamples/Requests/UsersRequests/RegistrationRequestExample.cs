namespace Blog.Web.SwaggerExamples.Requests.UsersRequests;

using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Requests.UsersRequests;
using Core.Consts;

/// <summary>
/// Registration request example.
/// </summary>
/// <seealso cref="IExamplesProvider{RegistrationRequest}" />
public class RegistrationRequestExample : IExamplesProvider<RegistrationRequest>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public RegistrationRequest GetExamples()
    {
        return new RegistrationRequest
        {
            Email = SwaggerExamplesConsts.AccountExample.Email,
            Password = SwaggerExamplesConsts.AccountExample.Password,
            ConfirmPassword = SwaggerExamplesConsts.AccountExample.Password,
            FirstName = SwaggerExamplesConsts.AccountExample.FirstName,
            LastName = SwaggerExamplesConsts.AccountExample.LastName,
            UserName = SwaggerExamplesConsts.AccountExample.UserName,
            PhoneNumber = SwaggerExamplesConsts.AccountExample.PhoneNumber,
            Roles = new List<string>
            {
                Consts.Roles.User
            }
        };
    }
}