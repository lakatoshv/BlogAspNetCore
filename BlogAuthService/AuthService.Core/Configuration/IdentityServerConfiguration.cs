namespace AuthService.Core.Configuration
{
    using System.Collections.Generic;
    using IdentityModel;
    using IdentityServer4.Models;

    /// <summary>
    /// Identity server configuration.
    /// </summary>
    public class IdentityServerConfiguration
    {
        /// <summary>
        /// Gets the apis.
        /// </summary>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("BlogApi"),
            };

        /// <summary>
        /// Gets the clients.
        /// </summary>
        /// <returns>IEnumerable.</returns>
        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client_id",
                    ClientSecrets =
                    {
                        new Secret("client_sectret".ToSha256()),
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "BlogApi" },
                },

                new Client
                {
                    ClientId = "client1",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("password".Sha256()),
                    },
                    AllowedScopes = { "BlogApi" },
                },

                new Client
                {
                    ClientId = "ropcclient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true, // Enables refresh token.
                    ClientSecrets =
                    {
                        new Secret("password".Sha256()),
                    },
                    AllowedScopes = { "BlogApi", },
                },
            };
    }
}
