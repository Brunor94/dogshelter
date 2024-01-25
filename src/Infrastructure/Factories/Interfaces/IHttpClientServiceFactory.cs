namespace DogShelterService.Infrastructure.Factories.Interfaces
{
    using DogShelterService.Infrastructure.Services.Interfaces;

    public interface IHttpClientServiceFactory
    {
        IHttpClientService CreateClient(string name);
    }
}