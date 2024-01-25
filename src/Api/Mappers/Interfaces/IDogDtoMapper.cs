namespace DogShelterService.Api.Mappers.Interfaces
{
    using System.Collections.Generic;
    using DogShelterService.Api.DTOs;
    using DogShelterService.Common.Mappers.Interfaces;
    using DogShelterService.Domain.Entities;

    public interface IDogDtoMapper :
        IMapper<Dog, DogDto>,
        IMapper<List<Dog>, List<DogDto>>
    {
    }
}