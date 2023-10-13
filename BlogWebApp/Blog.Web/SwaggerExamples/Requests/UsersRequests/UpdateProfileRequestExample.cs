namespace Blog.Web.SwaggerExamples.Requests.UsersRequests;

using Swashbuckle.AspNetCore.Filters;
using Blog.Contracts.V1.Requests.UsersRequests;
using Core.Consts;

/// <summary>
/// Update profile request example.
/// </summary>
/// <seealso cref="IExamplesProvider{UpdateProfileRequest}" />
public class UpdateProfileRequestExample : IExamplesProvider<UpdateProfileRequest>
{
    /// <inheritdoc cref="IExamplesProvider{T}"/>
    public UpdateProfileRequest GetExamples()
    {
        return new UpdateProfileRequest
        {
            Email = SwaggerExamplesConsts.UpdateProfileRequestExample.Email,
            FirstName = SwaggerExamplesConsts.UpdateProfileRequestExample.FirstName,
            LastName = SwaggerExamplesConsts.UpdateProfileRequestExample.LastName,
            PhoneNumber = SwaggerExamplesConsts.UpdateProfileRequestExample.PhoneNumber,
            Password = SwaggerExamplesConsts.UpdateProfileRequestExample.Password,
            About = SwaggerExamplesConsts.UpdateProfileRequestExample.About
        };
    }
}