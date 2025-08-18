namespace Blog.FSharp.Core.Constants

open Blog.FSharp.Core.Constants

/// <summary>
/// Swagger constants.
/// </summary>
module SwaggerConstants =

    /// <summary>
    /// The description.
    /// </summary>
    [<Literal>]
    let Description = Constants.ApplicationName + " Endpoints"

    /// <summary>
    /// The terms of service.
    /// </summary>
    [<Literal>]
    let TermsOfService = "https://example.com/terms"

    /// <summary>
    /// Contact.
    /// </summary>
    module Contact =
        /// <summary>
        /// The name.
        /// </summary>
        [<Literal>]
        let Name = "Vitalii Lakatosh"

        /// <summary>
        /// The email.
        /// </summary>
        // У F# літерал не може бути null → робимо як string option
        let Email: string option = None

        /// <summary>
        /// The URL.
        /// </summary>
        [<Literal>]
        let Url = "http://lakatoshv.byethost8.com/resume.php"

    /// <summary>
    /// License.
    /// </summary>
    module License =
        /// <summary>
        /// The name.
        /// </summary>
        [<Literal>]
        let Name = "Use under LICX"

        /// <summary>
        /// The URL.
        /// </summary>
        [<Literal>]
        let Url = "https://example.com/license"

    /// <summary>
    /// Security definition.
    /// </summary>
    module SecurityDefinition =
        /// <summary>
        /// The name.
        /// </summary>
        [<Literal>]
        let Name = "Bearer"

        /// <summary>
        /// Open api security scheme.
        /// </summary>
        module OpenApiSecurityScheme =
            /// <summary>
            /// The description.
            /// </summary>
            [<Literal>]
            let Description = "Jwt Authorization header using the bearer scheme"

            /// <summary>
            /// The name.
            /// </summary>
            [<Literal>]
            let Name = "Authorization"

            /// <summary>
            /// The scheme.
            /// </summary>
            [<Literal>]
            let Scheme = "bearer"

    /// <summary>
    /// Security requirement.
    /// </summary>
    module SecurityRequirement =
        /// <summary>
        /// Open api security requirement.
        /// </summary>
        module OpenApiSecurityRequirement =
            /// <summary>
            /// Open api reference.
            /// </summary>
            module OpenApiReference =
                /// <summary>
                /// The identifier.
                /// </summary>
                [<Literal>]
                let Id = "Bearer"