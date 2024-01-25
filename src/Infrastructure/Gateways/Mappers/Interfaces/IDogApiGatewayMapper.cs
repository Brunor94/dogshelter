namespace DogShelterService.Infrastructure.Gateways.Mappers.Interfaces
{
    using DogShelterService.Common.Mappers.Interfaces;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Infrastructure.Gateways.Models;

    public interface IDogApiGatewayMapper :
        IMapper<DogApiGatewayDTO, Breed>
    {
    }
}