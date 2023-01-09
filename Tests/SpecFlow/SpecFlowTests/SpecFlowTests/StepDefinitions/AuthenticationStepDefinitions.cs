using IdentityModel.Client;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class AuthenticationStepDefinitions
    {
        private FeatureContext _featureContext;

        public AuthenticationStepDefinitions(
            FeatureContext featureContext)
        {
            _featureContext = featureContext;
        }

        [Given(@"I am logged in as admin")]
        public async Task LoggedInAsAdmin()
        {
            _featureContext["accessToken"] = await GetAccessTokenAsync();
        }

        public static async Task<string> GetAccessTokenAsync()
        {
            var client = new HttpClient();
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = $"http://localhost:8080/identity-server/connect/token",
                ClientId = "admin",
                ClientSecret = "secret",
                Scope = "api"

            }).ConfigureAwait(false);
            tokenResponse.HttpResponse.EnsureSuccessStatusCode();

            return tokenResponse.AccessToken;
        }
    }
}
