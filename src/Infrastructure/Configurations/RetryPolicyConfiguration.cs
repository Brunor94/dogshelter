namespace DogShelterService.Infrastructure.Configurations
{
    using DogShelterService.Infrastructure.Configurations.Interfaces;

    public class RetryPolicyConfiguration : IRetryPolicyConfiguration
    {
        public int MaxRetry { get; set; }
        public int PauseBetweenFailuresInSeconds { get; set; }
    }
}