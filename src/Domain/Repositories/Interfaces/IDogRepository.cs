namespace DogShelterService.Domain.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Domain.Queries.Interfaces;

    public interface IDogRepository
    {
        Task<Dog> AddAsync(Dog dog, CancellationToken cancellationToken = default);

        Task<List<Dog>> GetAsync(ISearchDogsRequest searchDogsRequest, CancellationToken cancellationToken = default);
    }
}