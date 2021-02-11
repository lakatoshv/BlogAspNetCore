// <copyright file="JwtClaimTypes.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Blog.Services.Core.Identity.Auth
{
    /// <summary>
    /// Jwt claim types.
    /// </summary>
    public class JwtClaimTypes
    {
        /// <summary>
        /// Rol.
        /// </summary>
        public const string Rol = "rol";

        /// <summary>
        /// Id.
        /// </summary>
        public const string Id = "id";

        /// <summary>
        /// UserName.
        /// </summary>
        public const string UserName = "userName";

        /// <summary>
        /// Email.
        /// </summary>
        public const string Email = "email";

        /// <summary>
        /// PhoneNumber.
        /// </summary>
        public const string PhoneNumber = "phoneNumber";

        /// <summary>
        /// IsEmailVerified.
        /// </summary>
        public const string IsEmailVerified = "isEmailVerified";

        /// <summary>
        /// The profile identifier.
        /// </summary>
        public const string ProfileId = "profileId";
    }
}
