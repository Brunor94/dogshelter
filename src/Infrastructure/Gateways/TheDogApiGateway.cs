namespace DogShelterService.Infrastructure.Gateways
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DogShelterService.Common.Services.Interfaces;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Domain.Gateways.Interfaces;
    using DogShelterService.Infrastructure.Factories.Interfaces;
    using DogShelterService.Infrastructure.Gateways.Mappers.Interfaces;
    using DogShelterService.Infrastructure.Gateways.Models;
    using DogShelterService.Infrastructure.Services.Interfaces;
    using EnsureThat;

    public sealed class TheDogApiGateway : ITheDogApiGateway
    {
        private readonly IDogApiGatewayMapper dogApiGatewayMapper;
        private readonly IRetryService retryService;
        private readonly IHttpClientService httpClientService;

        public TheDogApiGateway(IHttpClientServiceFactory httpClientServiceFactory, IDogApiGatewayMapper dogApiGatewayMapper, IRetryService retryService)
        {
            EnsureArg.IsNotNull(httpClientServiceFactory);
            EnsureArg.IsNotNull(dogApiGatewayMapper);
            EnsureArg.IsNotNull(retryService);

            this.dogApiGatewayMapper = dogApiGatewayMapper;
            this.retryService = retryService;
            this.httpClientService = httpClientServiceFactory.CreateClient(nameof(TheDogApiGateway));
        }

        public async Task<Breed> GetBreedInfoAsync(string dogBreed, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(dogBreed);

            var requestUri = $"breeds/search?q={Uri.EscapeDataString(dogBreed)}";

            var breedsInfo = await this.retryService.RetryAsync(
                async token => await this.httpClientService.GetAsync<IEnumerable<DogApiGatewayDTO>>(requestUri, token),
                cancellationToken);

            return this.dogApiGatewayMapper.Map(breedsInfo.FirstOrDefault());
        }
    }
}