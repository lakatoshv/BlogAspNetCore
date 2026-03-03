namespace BlogMinimalApi.SwaggerExamples.Requests.UsersRequests;

using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Requests.UsersRequests;
using Blog.Core.Consts;

/// <summary>
/// Login request example.
/// </summary>
/// <seealso cref="IExamplesProvider{LoginRequest}" />
public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public LoginRequest GetExamples()
    {
        return new LoginRequest
        {
            Email = SwaggerExamplesConsts.AccountExample.Email,
            Password = SwaggerExamplesConsts.AccountExample.Password,
        };
    }
}