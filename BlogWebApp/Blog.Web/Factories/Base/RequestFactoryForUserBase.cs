namespace Blog.Web.Factories.Base;

using Contracts.V1.Requests.Interfaces;

/// <summary>
/// Request factory for user base.
/// </summary>
/// <typeparam name="TChangePasswordRequest">The type of the change password request.</typeparam>
/// <typeparam name="TLoginRequest">The type of the login request.</typeparam>
/// <typeparam name="TRegistrationRequest">The type of the registration request.</typeparam>
/// <typeparam name="TUpdateRequest">The type of the update request.</typeparam>
public abstract class RequestFactoryForUserBase<TChangePasswordRequest, TLoginRequest, TRegistrationRequest, TUpdateRequest>
    where TChangePasswordRequest : IRequest, new()
    where TLoginRequest : IRequest, new()
    where TRegistrationRequest : IRequest, new()
    where TUpdateRequest : IRequest, new()
{
    /// <summary>
    /// Generates the change password request.
    /// </summary>
    /// <returns>TChangePasswordRequest.</returns>
    public abstract TChangePasswordRequest GenerateChangePasswordRequest();

    /// <summary>
    /// Generates the login request.
    /// </summary>
    /// <returns>TLoginRequest.</returns>
    public abstract TLoginRequest GenerateLoginRequest();

    /// <summary>
    /// Generates the registration request.
    /// </summary>
    /// <returns>TRegistrationRequest.</returns>
    public abstract TRegistrationRequest GenerateRegistrationRequest();

    /// <summary>
    /// Generates the update request.
    /// </summary>
    /// <returns>TUpdateRequest.</returns>
    public abstract TUpdateRequest GenerateUpdateRequest();
}