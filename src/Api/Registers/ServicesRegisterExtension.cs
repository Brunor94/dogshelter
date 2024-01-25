namespace DogShelterService.Api.Registers
{
    using DogShelterService.Application.Services;
    using DogShelterService.Application.Services.Interfaces;
    using DogShelterService.Infrastructure.Factories;
    using DogShelterService.Infrastructure.Factories.Interfaces;
    using DogShelterService.Infrastructure.Services;
    using DogShelterService.Infrastructure.Services.Interfaces;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServicesRegisterExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IDogService, DogService>();

            services.AddSingleton<IHttpClientServiceFactory, HttpClientServiceFactory>();

            services.AddScoped<IHttpClientService, HttpClientService>();

            return services;
        }
    }
}