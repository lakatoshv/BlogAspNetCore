namespace Blog.Web.Factories
{
    using Blog.Contracts.V1.Requests.UsersRequests;
    using Blog.Web.Factories.Base;
    using System.Collections.Generic;

    /// <summary>
    /// User request factory.
    /// </summary>
    /// <seealso cref="UpdateProfileRequest" />
    public class UserRequestFactory : RequestFactoryForUserBase<ChangePasswordRequest, LoginRequest, RegistrationRequest, UpdateProfileRequest>
    {
        public override ChangePasswordRequest GenerateChangePasswordRequest() =>
            new ()
            {
                OldPassword = "OldPassword123",
                NewPassword = "NewPassword123",
            };

        public override LoginRequest GenerateLoginRequest() =>
            new()
            {
                Email = "EmailFromFactory@gmail.com",
                Password = "PasswordFromFactory123",
            };

        public override RegistrationRequest GenerateRegistrationRequest() =>
            new ()
            {
                Email = "EmailFromFactory@gmail.com",
                Password = "PasswordFromFactory123",
                ConfirmPassword = "PasswordFromFactory123",
                UserName = "EmailFromFactory@gmail.com",
                FirstName = "UserFromFactory",
                LastName = "UserFromFactory",
                PhoneNumber = "0123456789",
                Roles = new List<string>
                {
                    "User",
                },
            };

        public override UpdateProfileRequest GenerateUpdateRequest() =>
            new ()
            {
                Email = "EditedEmailFromFactory@gmail.com",
                FirstName = "EditedUserFromFactory",
                LastName = "EditedUserFromFactory",
                PhoneNumber = "0123456710",
                Password = "PasswordFromFactory123",
                About = "Edited about me.",
            };
    }
}