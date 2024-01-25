namespace DogShelterService.Infrastructure.Services.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IHttpClientService
    {
        Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken = default);
    }
}