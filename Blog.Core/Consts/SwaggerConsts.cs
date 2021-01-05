namespace Blog.Core.Consts
{
    /// <summary>
    /// Swagger constants.
    /// </summary>
    public class SwaggerConsts
    {
        /// <summary>
        /// The description.
        /// </summary>
        public const string Description = Consts.ApplicationName + " Endpoints";

        /// <summary>
        /// The terms of service.
        /// </summary>
        public const string TermsOfService = "https://example.com/terms";

        /// <summary>
        /// Contact.
        /// </summary>
        public static class Contact
        {
            /// <summary>
            /// The name.
            /// </summary>
            public const string Name = "Vitalii Lakatosh";

            /// <summary>
            /// The email.
            /// </summary>
            public const string Email = null;

            /// <summary>
            /// The URL.
            /// </summary>
            public const string Url = "http://lakatoshv.byethost8.com/resume.php";
        }

        /// <summary>
        /// License.
        /// </summary>
        public static class License
        {
            /// <summary>
            /// The name.
            /// </summary>
            public const string Name = "Use under LICX";

            /// <summary>
            /// The URL.
            /// </summary>
            public const string Url = "https://example.com/license";
        }

        /// <summary>
        /// Security definition.
        /// </summary>
        public static class SecurityDefinition
        {
            /// <summary>
            /// The name.
            /// </summary>
            public const string Name = "Bearer";

            /// <summary>
            /// Open api security scheme.
            /// </summary>
            public static class OpenApiSecurityScheme
            {
                /// <summary>
                /// The description.
                /// </summary>
                public const string Description = "Jwt Authorization header using the bearer scheme";

                /// <summary>
                /// The name.
                /// </summary>
                public const string Name = "Authorization";

                /// <summary>
                /// The scheme.
                /// </summary>
                public const string Scheme = "bearer";
            }
        }

        /// <summary>
        /// Security requirement.
        /// </summary>
        public static class SecurityRequirement
        {
            /// <summary>
            /// Open api security requirement.
            /// </summary>
            public static class OpenApiSecurityRequirement
            {
                /// <summary>
                /// Open api reference.
                /// </summary>
                public static class OpenApiReference
                {
                    /// <summary>
                    /// The identifier.
                    /// </summary>
                    public const string Id = "Bearer";
                }
            }
        }
    }
}