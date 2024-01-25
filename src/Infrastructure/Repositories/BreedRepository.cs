namespace DogShelterService.Infrastructure.Repositories
{
    using System.Threading;
    using System.Threading.Tasks;
    using DogShelterService.Common.Services.Interfaces;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Domain.Repositories.Interfaces;
    using DogShelterService.Infrastructure.Persistence;
    using DogShelterService.Infrastructure.Repositories.Mappers.Interfaces;
    using EnsureThat;
    using Microsoft.EntityFrameworkCore;

    public sealed class BreedRepository : IBreedRepository
    {
        private readonly DogShelterDbContext context;
        private readonly IBreedEntityMapper breedEntityMapper;
        private readonly IRetryService retryService;

        public BreedRepository(DogShelterDbContext context, IRetryService retryService, IBreedEntityMapper breedMapper)
        {
            EnsureArg.IsNotNull(context, nameof(context));
            EnsureArg.IsNotNull(retryService, nameof(retryService));
            EnsureArg.IsNotNull(breedMapper, nameof(breedMapper));

            this.context = context;
            this.retryService = retryService;
            this.breedEntityMapper = breedMapper;
        }

        public async Task<Breed> GetBreedByNameAsync(string breedName, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(breedName);

            var breed = await this.retryService.RetryAsync(
                async token => await this.context.Breeds.FirstOrDefaultAsync(breed => breed.Name == breedName, token),
                cancellationToken);

            return this.breedEntityMapper.Map(breed);
        }
    }
}