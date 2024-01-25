namespace DogShelterService.Application.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DogShelterService.Application.DTOs;
    using DogShelterService.Domain.Entities;
    using DogShelterService.Domain.Queries.Interfaces;

    public interface IDogService
    {
        Task<Dog> PostDogAsync(DogCreationRequestDTO dogCreationRequest, CancellationToken cancellationToken = default);

        Task<List<Dog>> GetDogsAsync(ISearchDogsRequest searchDogsRequest, CancellationToken cancellationToken = default);
    }
}