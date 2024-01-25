namespace DogShelterService.Domain.Gateways.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using DogShelterService.Domain.Entities;

    public interface ITheDogApiGateway
    {
        Task<Breed> GetBreedInfoAsync(string dogBreed, CancellationToken cancellationToken = default);
    }
}