namespace DogShelterService.Infrastructure.Repositories.Mappers.Interfaces
{
    using System.Collections.Generic;
    using DogShelterService.Common.Mappers.Interfaces;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Infrastructure.Repositories.Entities;

    public interface IDogEntityMapper :
        IMapper<DogEntity, Dog>,
        IMapper<List<DogEntity>, List<Dog>>,
        IMapper<Dog, DogEntity>
    {
    }
}