using Duende.IdentityServer.Models;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
            new ApiScope("api", "API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
            new Client
            {
                ClientId = "admin",

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                AllowedScopes = { "api" },

                Claims = new List<ClientClaim>
                {
                    new ClientClaim ("sub", "68d76b61-2532-4aa9-bc68-733e65d878b9")
                }
            }
            };
    }
}
