namespace DogShelterService.Api.Registers
{
    using DogShelterService.Domain.Gateways.Interfaces;
    using DogShelterService.Infrastructure.Gateways;
    using Microsoft.Extensions.DependencyInjection;

    public static class GatewaysRegisterExtension
    {
        public static IServiceCollection RegisterGateways(this IServiceCollection services)
        {
            services.AddScoped<ITheDogApiGateway, TheDogApiGateway>();

            return services;
        }
    }
}