namespace DogShelterService.Infrastructure.Services
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using DogShelterService.Infrastructure.Services.Interfaces;
    using EnsureThat;

    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            EnsureArg.IsNotNull(httpClient);

            this.httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(requestUri);

            var response = await httpClient.GetAsync(requestUri, cancellationToken);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}