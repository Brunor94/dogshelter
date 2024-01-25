namespace DogShelterService.Infrastructure.Repositories.Mappers.Interfaces
{
    using DogShelterService.Common.Mappers.Interfaces;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Infrastructure.Repositories.Entities;

    public interface IBreedEntityMapper :
        IMapper<BreedEntity, Breed>,
        IMapper<Breed, BreedEntity>
    {
    }
}