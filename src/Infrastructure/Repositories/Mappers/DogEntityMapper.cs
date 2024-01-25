namespace DogShelterService.Infrastructure.Repositories.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Infrastructure.Repositories.Entities;
    using DogShelterService.Infrastructure.Repositories.Mappers.Interfaces;
    using EnsureThat;

    public sealed class DogEntityMapper : IDogEntityMapper
    {
        private readonly IBreedEntityMapper breedEntityMapper;

        public DogEntityMapper(IBreedEntityMapper breedEntityMapper)
        {
            EnsureArg.IsNotNull(breedEntityMapper, nameof(breedEntityMapper));

            this.breedEntityMapper = breedEntityMapper;
        }

        public Dog Map(DogEntity dogEntity)
        {
            if (dogEntity is null)
            {
                return null;
            }

            return new Dog()
            {
                Id = dogEntity.Id,
                Name = dogEntity.Name,
                Breed = this.breedEntityMapper.Map(dogEntity.Breed)
            };
        }

        public DogEntity Map(Dog dog)
        {
            if (dog is null)
            {
                return null;
            }

            var dogEntity = new DogEntity()
            {
                Id = dog.Id,
                Name = dog.Name,
            };

            if (dog.Breed.Id != default)
            {
                dogEntity.BreedId = dog.Breed.Id;
            }
            else
            {
                dogEntity.Breed = this.breedEntityMapper.Map(dog.Breed);
            }

            return dogEntity;
        }

        public List<Dog> Map(List<DogEntity> dogRepositoriesDTO)
        {
            if (!dogRepositoriesDTO.Any())
            {
                return new List<Dog>();
            }

            return dogRepositoriesDTO.Select(this.Map).ToList();
        }
    }
}