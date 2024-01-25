namespace DogShelterService.Api.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using DogShelterService.Api.DTOs;
    using DogShelterService.Api.Mappers.Interfaces;
    using DogShelterService.Domain.Entities;

    public class DogDtoMapper : IDogDtoMapper
    {
        public DogDto Map(Dog dog)
        {
            if (dog is null)
            {
                return null;
            }

            return new DogDto()
            {
                Name = dog.Name,
                Breed = dog.Breed.Name,
                AvgHeight = dog.Breed.AvgHeight,
                Temperaments = dog.Breed.Temperaments
            };
        }

        public List<DogDto> Map(List<Dog> dogs)
        {
            if (!dogs.Any())
            {
                return new List<DogDto>();
            }

            return dogs.Select(this.Map).ToList();
        }
    }
}