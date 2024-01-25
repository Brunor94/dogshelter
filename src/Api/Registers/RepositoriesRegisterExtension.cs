namespace DogShelterService.Api.Registers
{
    using DogShelterService.Domain.Repositories.Interfaces;
    using DogShelterService.Infrastructure.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class RepositoriesRegisterExtension
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBreedRepository, BreedRepository>();
            services.AddScoped<IDogRepository, DogRepository>();

            return services;
        }
    }
}