using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class IdentityConfigs
    {
        //we tell identity server to support openid 
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "extraprofile.scope",
                    UserClaims =
                    {
                        "DisplayName"
                    }
                }
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource("TravelogApi")
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client> { 
                new Client
            {
                ClientId = "travelog_api_client",
                ClientSecrets = { new Secret("travelog_api_secret".ToSha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials, //how to retrieve tokens,
                AllowedScopes = { "TravelogApi"}, //what can this client access
                RequireConsent = false
            },
                new Client
            {
                ClientId = "travelog_react_client",
                AllowedGrantTypes = GrantTypes.Code, //how to retrieve tokens
                RequirePkce = true,
                RequireClientSecret = false,
                AllowedScopes = { "openid", "TravelogApi", "profile", "extraprofile.scope" }, //what can this client access
                RedirectUris = {"http://localhost:3000/auth/signin-oidc", "http://localhost:3000/auth/signin-silent-oidc" }, //what we specified in our js client
                PostLogoutRedirectUris = {"https://localhost:3000/" }, //what we specified in our js client
                RequireConsent = false,
                AllowAccessTokensViaBrowser = true,
                AllowedCorsOrigins = { "http://localhost:3000"},
                AlwaysIncludeUserClaimsInIdToken = true
            }
            };
    }
}
