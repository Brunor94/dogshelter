namespace DogShelterService.Domain.Repositories.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using DogShelterService.Domain.Entities;

    public interface IBreedRepository
    {
        Task<Breed> GetBreedByNameAsync(string breedName, CancellationToken cancellationToken = default);
    }
}