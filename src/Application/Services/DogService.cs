namespace DogShelterService.Application.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DogShelterService.Application.DTOs;
    using DogShelterService.Application.Services.Interfaces;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Domain.Gateways.Interfaces;
    using DogShelterService.Domain.Queries.Interfaces;
    using DogShelterService.Domain.Repositories.Interfaces;
    using EnsureThat;

    public sealed class DogService : IDogService
    {
        private readonly ITheDogApiGateway theDogApiGateway;
        private readonly IDogRepository dogRepository;
        private readonly IBreedRepository breedRepository;
        private readonly IUnitOfWork unitOfWork;

        public DogService(
            ITheDogApiGateway theDogApiGateway,
            IDogRepository dogRepository,
            IBreedRepository breedRepository,
            IUnitOfWork unitOfWork)
        {
            EnsureArg.IsNotNull(theDogApiGateway, nameof(theDogApiGateway));
            EnsureArg.IsNotNull(dogRepository, nameof(dogRepository));
            EnsureArg.IsNotNull(breedRepository, nameof(breedRepository));
            EnsureArg.IsNotNull(unitOfWork, nameof(unitOfWork));

            this.theDogApiGateway = theDogApiGateway;
            this.dogRepository = dogRepository;
            this.breedRepository = breedRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Dog> PostDogAsync(DogCreationRequestDTO dogCreationRequest, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(dogCreationRequest);

            var breed = await this.theDogApiGateway.GetBreedInfoAsync(dogCreationRequest.Breed, cancellationToken);

            if (breed is null)
            {
                return null;
            }

            var foundBreed = await this.breedRepository.GetBreedByNameAsync(breed.Name, cancellationToken);

            var newDog = new Dog()
            {
                Name = dogCreationRequest.Name,
                Breed = foundBreed ??= breed
            };

            newDog = await this.dogRepository.AddAsync(newDog, cancellationToken);

            var affectedRows = await this.unitOfWork.CommitAsync(cancellationToken);

            return affectedRows > 0 ? newDog : null;
        }

        public async Task<List<Dog>> GetDogsAsync(ISearchDogsRequest searchDogsRequest, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(searchDogsRequest);

            return await this.dogRepository.GetAsync(searchDogsRequest, cancellationToken);
        }
    }
}