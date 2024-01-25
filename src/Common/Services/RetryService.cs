namespace DogShelterService.Common.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DogShelterService.Common.Services.Interfaces;
    using Polly;
    using Polly.Retry;

    public class RetryService : IRetryService
    {
        private readonly AsyncRetryPolicy asyncRetryPolicy;

        public RetryService(int retryCount, TimeSpan retryInterval)
        {
            asyncRetryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(retryCount, _ => retryInterval);
        }

        public Task<TResult> RetryAsync<TResult>(Func<CancellationToken, Task<TResult>> operation, CancellationToken cancellationToken)
        {
            return asyncRetryPolicy.ExecuteAsync(ct => operation(ct), cancellationToken);
        }
    }
}