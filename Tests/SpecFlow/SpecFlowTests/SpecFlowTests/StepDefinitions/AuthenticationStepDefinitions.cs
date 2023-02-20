using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class AuthenticationStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        private string _identityServerUrl;

        public AuthenticationStepDefinitions(ScenarioContext scenarioContext, IOptions<TestSettings> testSettings)
        {
            _scenarioContext = scenarioContext;
            _identityServerUrl = testSettings.Value.IdentityServerUrl;
        }

        [Given(@"I am logged in as admin")]
        public async Task LoggedInAsAdmin()
        {
            _scenarioContext["accessToken"] = await GetAccessTokenAsync("admin", _identityServerUrl);
        }

        [Given(@"I am logged in as user without permissions")]
        public async Task LoggedInAsUser()
        {
            _scenarioContext["accessToken"] = await GetAccessTokenAsync("user", _identityServerUrl);
        }

        public static async Task<string> GetAccessTokenAsync(string clientId, string ingressUrl)
        {
            var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = $"{ingressUrl}/connect/token",
                ClientId = clientId,
                ClientSecret = "secret",
                Scope = "api"

            }).ConfigureAwait(false);
            tokenResponse.HttpResponse.EnsureSuccessStatusCode();

            return tokenResponse.AccessToken;
        }
    }
}
