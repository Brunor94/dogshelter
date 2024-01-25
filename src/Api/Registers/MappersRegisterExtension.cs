namespace DogShelterService.Api.Registers
{
    using DogShelterService.Api.Mappers;
    using DogShelterService.Api.Mappers.Interfaces;
    using DogShelterService.Infrastructure.Gateways.Mappers;
    using DogShelterService.Infrastructure.Gateways.Mappers.Interfaces;
    using DogShelterService.Infrastructure.Repositories.Mappers;
    using DogShelterService.Infrastructure.Repositories.Mappers.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    public static class MappersRegisterExtension
    {
        public static IServiceCollection RegisterMappers(this IServiceCollection services)
        {
            services.AddSingleton<IDogDtoMapper, DogDtoMapper>();

            services.AddSingleton<IDogApiGatewayMapper, DogApiGatewayMapper>();

            services.AddSingleton<IDogEntityMapper, DogEntityMapper>();
            services.AddSingleton<IBreedEntityMapper, BreedEntityMapper>();

            return services;
        }
    }
}