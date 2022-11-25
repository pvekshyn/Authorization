using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Monitoring.Logic
{
    public interface IPubSubQuery
    {
        Task<int> GetMessagesCountAsync();
    }

    public class PubSubQuery : IPubSubQuery
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PubSubQuery(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<int> GetMessagesCountAsync()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:15672/api/queues/%2f/authorizationQueue");

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"guest:guest")));
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();

            var result = await JsonSerializer.DeserializeAsync<RabbitResponse>(contentStream);
            return result.messages;
        }

        private class RabbitResponse
        {
            public int messages { get; set; }
        }
    }
}
