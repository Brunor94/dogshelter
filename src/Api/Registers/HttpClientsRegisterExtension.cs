namespace DogShelterService.Api.Registers
{
    using System;
    using DogShelterService.Infrastructure.Configurations;
    using DogShelterService.Infrastructure.Configurations.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    public static class HttpClientsRegisterExtension
    {
        public static IServiceCollection RegisterHttpClients(this IServiceCollection services)
        {
            var gatewaysConfiguration = services.BuildServiceProvider()
                                                .GetRequiredService<IGatewaysSettings>();

            services.AddHttpClient(nameof(GatewaysSettings.TheDogApiGateway), c =>
            {
                c.BaseAddress = new Uri(gatewaysConfiguration.TheDogApiGateway.BaseUrl);
            });

            return services;
        }
    }
}