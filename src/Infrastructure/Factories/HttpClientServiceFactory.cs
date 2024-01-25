namespace DogShelterService.Infrastructure.Factories
{
    using System;
    using System.Net.Http;
    using DogShelterService.Infrastructure.Factories.Interfaces;
    using DogShelterService.Infrastructure.Services;
    using DogShelterService.Infrastructure.Services.Interfaces;
    using EnsureThat;

    public class HttpClientServiceFactory : IHttpClientServiceFactory
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpClientServiceFactory(IHttpClientFactory httpClientFactory)
        {
            EnsureArg.IsNotNull(httpClientFactory);

            this.httpClientFactory = httpClientFactory;
        }

        public IHttpClientService CreateClient(string name)
        {
            EnsureArg.IsNotNullOrWhiteSpace(name);

            var httpClient = this.httpClientFactory.CreateClient(name);

            if (httpClient == null)
            {
                throw new InvalidOperationException($"Failed to create an HTTP client for '{name}'");
            }

            return new HttpClientService(httpClient);
        }
    }
}