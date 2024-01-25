namespace DogShelterService.Infrastructure.Repositories.Mappers
{
    using System.Linq;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Domain.Enums;
    using DogShelterService.Infrastructure.Repositories.Entities;
    using DogShelterService.Infrastructure.Repositories.Mappers.Interfaces;

    public sealed class BreedEntityMapper : IBreedEntityMapper
    {
        public Breed Map(BreedEntity breedEntity)
        {
            if (breedEntity is null)
            {
                return null;
            }

            return new Breed()
            {
                Id = breedEntity.Id,
                Name = breedEntity.Name,
                SizeCategory = (SizeCategory)breedEntity.SizeCategory,
                Temperaments = breedEntity.Temperaments.Split(',').ToList(),
                AvgHeight = breedEntity.AvgHeight
            };
        }

        public BreedEntity Map(Breed breed)
        {
            if (breed is null)
            {
                return null;
            }

            return new BreedEntity()
            {
                Id = breed.Id,
                Name = breed.Name,
                SizeCategory = (int)breed.SizeCategory,
                Temperaments = string.Join(',', breed.Temperaments),
                AvgHeight = breed.AvgHeight
            };
        }
    }
}