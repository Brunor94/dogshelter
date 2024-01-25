namespace DogShelterService.Common.Services.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRetryService
    {
        Task<TResult> RetryAsync<TResult>(Func<CancellationToken, Task<TResult>> operation, CancellationToken cancellationToken = default);
    }
}